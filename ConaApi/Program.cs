using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;
using ConaApi.Models;
using ConaApi.Services;
using Swashbuckle.AspNetCore.SwaggerUI;
using Serilog;
using Serilog.Events;

namespace ConaApi;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/cona-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Starting up Cona API");
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Cona API",
                    Version = "v1",
                    Description = "API for Cona application"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Configure JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ??
                            throw new InvalidOperationException("JWT Key not found in configuration")))
                };
            });

            // Configure MongoDB
            builder.Services.AddSingleton<IMongoClient>(sp =>
                new MongoClient(builder.Configuration.GetConnectionString("MongoDB") ?? "mongodb://localhost:27017"));

            builder.Services.AddSingleton<IMongoDatabase>(sp =>
                sp.GetRequiredService<IMongoClient>().GetDatabase("ConaDB"));

            // Register MongoDB collections
            builder.Services.AddSingleton<IMongoCollection<IngredientAnalysis>>(sp =>
                sp.GetRequiredService<IMongoDatabase>().GetCollection<IngredientAnalysis>("ingredientAnalyses"));
            builder.Services.AddSingleton<IMongoCollection<Recipe>>(sp =>
                sp.GetRequiredService<IMongoDatabase>().GetCollection<Recipe>("recipes"));
            builder.Services.AddSingleton<IMongoCollection<CachedIngredient>>(sp =>
                sp.GetRequiredService<IMongoDatabase>().GetCollection<CachedIngredient>("cachedIngredients"));

            builder.Services.AddSingleton<NutritionService>();
            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Configure middleware pipeline
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cona API V1");
                c.RoutePrefix = "swagger";
                c.DocExpansion(DocExpansion.None);
            });

            // Add redirect from /swagger to root
            app.MapGet("/swagger", () => Results.Redirect("swagger/index.html"));

            // Add redirect from root to swagger UI
            app.MapGet("/", () => Results.Redirect("swagger/index.html"));

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application start-up failed");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
