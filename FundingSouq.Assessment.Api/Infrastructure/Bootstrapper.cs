using System.Text;
using FluentValidation;
using FundingSouq.Assessment.Api.Infrastructure;
using FundingSouq.Assessment.Application.Behaviors;
using FundingSouq.Assessment.Application.Commands;
using FundingSouq.Assessment.Application.Services;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Interfaces;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using FundingSouq.Assessment.Infrastructure.Contexts;
using FundingSouq.Assessment.Infrastructure.Interceptors;
using FundingSouq.Assessment.Infrastructure.Repositories;
using FundingSouq.Assessment.Infrastructure.Seeder;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace FundingSouq.Assessment.Api;

/// <summary>
/// Bootstrapper class containing extension methods to register and configure application services and infrastructure.
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Registers the application's infrastructure services, including the database context, caching, and distributed locking.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> used to configure the application.</param>
    public static void RegisterApplicationInfrastructure(this WebApplicationBuilder builder)
    {
        // Register SaveChangesInterceptor to handle base entity properties updates like CreatedAt, LastModifiedAt
        builder.Services.AddScoped<ISaveChangesInterceptor, BaseEntityInterceptor>();

        // Configure the database context with Npgsql and snake_case naming convention
        builder.Services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options
                .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>())
                .UseSnakeCaseNamingConvention();
        });

        // Register UnitOfWork as a scoped service
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Configure Redis cache
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "redis-cache";
        });
        
        // Configure output caching with a base expiration policy of 5 minutes
        builder.Services.AddOutputCache(options =>
        {
            options.AddBasePolicy(config => 
                config.Expire(TimeSpan.FromMinutes(5))
            );
        });
        
        // Configure Redis-based output cache
        builder.Services.AddStackExchangeRedisOutputCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "redis-output-cache";
        });
        
        // Configure distributed locking using Redis and RedLock
        builder.Services.AddSingleton<IDistributedLockFactory>(_ =>
        {
            var redisConnectionMultiplexer = ConnectionMultiplexer
                .Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
    
            var redLockMultiplexer = new List<RedLockMultiplexer>
            {
                new(redisConnectionMultiplexer)
            };

            return RedLockFactory.Create(redLockMultiplexer);
        });
    }

    /// <summary>
    /// Registers application services, including validators, MediatR, authentication, and authorization.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> used to configure the application.</param>
    public static void RegisterApplicationServices(this WebApplicationBuilder builder)
    {
        // Register FluentValidation validators from the specified assembly
        builder.Services.AddValidatorsFromAssembly(typeof(CreateClientCommandValidator).Assembly);

        // Register HttpContextAccessor for accessing HTTP context in services
        builder.Services.AddHttpContextAccessor();

        // Register MediatR with a custom validation behavior
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(RegisterHubUserCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
        
        // Configure authorization policies for HubUser and Client roles
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(
                nameof(UserType.HubUser),
                policy => policy.RequireClaim("user_type", nameof(UserType.HubUser)));
            
            options.AddPolicy(
                nameof(UserType.Client),
                policy => policy.RequireClaim("user_type", nameof(UserType.Client)));
        });

        // Configure JWT authentication
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
                    ValidAudience = builder.Configuration["JwtOptions:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Key"]!))
                };
            });
        
        // Bind the Globals configuration section to the Globals class
        builder.Services.Configure<Globals>(builder.Configuration.GetSection("Globals"));
        
        // Register custom global exception handler
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        
        // Register application-specific services
        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<IFileUploadService, FileUploadService>();
    }

    /// <summary>
    /// Applies any pending migrations to the database and seeds initial data.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> used to configure the application pipeline.</param>
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Apply any pending migrations
        dbContext.Database.Migrate();
        
        // Seed the database with initial data
        var seeder = new DatabaseSeeder(dbContext, scope.ServiceProvider.GetRequiredService<IPasswordHasher>());
        seeder.Seed().Wait();
    }
}
