using FundingSouq.Assessment.Api;
using FundingSouq.Assessment.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configure controllers and JSON settings
builder.Services
    .AddControllers(opt =>
    {
        // Use kebab-case for routes
        opt.Conventions.Add(new RouteTokenTransformerConvention(new KebabParameterTransformer()));
    })
    .AddNewtonsoftJson(options =>
    {
        // Handle reference loops by ignoring them and omit null values in JSON responses
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });

// Register services for API exploration and Swagger documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Add security definition for Bearer token authentication
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                    Enter 'Bearer' [space] and then your token in the text input below.
                    Example: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    // Apply security requirement globally to all API endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            Array.Empty<string>()
        }
    });

    // Add operation filter to include security requirements in Swagger UI
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Register application-specific services and infrastructure
builder.RegisterApplicationServices();
builder.RegisterApplicationInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Enable Swagger in development environment
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FundingSouq.Assessment.Api v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });

    // Apply any pending migrations to the database during development
    app.ApplyMigrations();
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable output caching for responses
app.UseOutputCache();

var filesDirectoryPath = Path.Combine(builder.Environment.ContentRootPath, "files");
if (!Directory.Exists(filesDirectoryPath))
{
    // Ensure the files directory exists
    Directory.CreateDirectory(filesDirectoryPath);
}

// Serve static files from the /files path
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(filesDirectoryPath),
    RequestPath = "/files"
});

// Configure authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Use a global exception handler
app.UseExceptionHandler();

// Map controller routes
app.MapControllers();

app.Run();
