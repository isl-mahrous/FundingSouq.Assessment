using System.Reflection;
using FluentValidation;
using FundingSouq.Assessment.Application.Services;
using FundingSouq.Assessment.Application.Validators;
using FundingSouq.Assessment.Core.Interfaces;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using FundingSouq.Assessment.Infrastructure.Contexts;
using FundingSouq.Assessment.Infrastructure.Interceptors;
using FundingSouq.Assessment.Infrastructure.Repositories;
using FundingSouq.Assessment.Infrastructure.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FundingSouq.Assessment.Api.Extensions;

public static class Bootstrapper
{
    public static void RegisterApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISaveChangesInterceptor, BaseEntityInterceptor>();
        builder.Services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options
                .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>())
                .UseSnakeCaseNamingConvention();
        });

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
        });

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        
        builder.Services.AddValidatorsFromAssembly(typeof(RegisterValidator).Assembly);
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