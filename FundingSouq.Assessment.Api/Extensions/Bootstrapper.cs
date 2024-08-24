using System.Text;
using FluentValidation;
using FundingSouq.Assessment.Api.Infrastructure;
using FundingSouq.Assessment.Application.Behaviors;
using FundingSouq.Assessment.Application.Commands;
using FundingSouq.Assessment.Application.Services;
using FundingSouq.Assessment.Application.Validators;
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

namespace FundingSouq.Assessment.Api.Extensions;

public static class Bootstrapper
{
    public static void RegisterApplicationInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISaveChangesInterceptor, BaseEntityInterceptor>();
        builder.Services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options
                .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>())
                .UseSnakeCaseNamingConvention();
        });
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Redis Cache
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "redis-cache";
        });
        
        // Output Cache
        builder.Services.AddOutputCache(options =>
        {
            options.AddBasePolicy(config => 
                config.Expire(TimeSpan.FromMinutes(5))
            );
        });
        
        builder.Services.AddStackExchangeRedisOutputCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "redis-output-cache";
        });
        
        // Distributed Lock with Redis and RedLock
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

    public static void RegisterApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(typeof(RegisterValidator).Assembly);

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(RegisterHubUserCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
        
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(
                nameof(UserType.HubUser),
                policy => policy.RequireClaim("user_type", nameof(UserType.HubUser)));
            
            options.AddPolicy(
                nameof(UserType.Client),
                policy => policy.RequireClaim("user_type", nameof(UserType.Client)));
        });

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
        
        builder.Services.Configure<Globals>(builder.Configuration.GetSection("Globals"));
        
        
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<IFileUploadService, FileUploadService>();
    }

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        dbContext.Database.Migrate();
        
        var seeder = new DatabaseSeeder(dbContext, scope.ServiceProvider.GetRequiredService<IPasswordHasher>());
        seeder.Seed().Wait();
    }
}