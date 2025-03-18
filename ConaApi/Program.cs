using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using ConaApi.Models;
using ConaApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure JWT Authentication
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

// Add MongoDB service
builder.Services.AddSingleton<IMongoClient>(sp => 
    new MongoClient("mongodb://localhost:27017"));

builder.Services.AddSingleton<IMongoDatabase>(sp => 
    sp.GetRequiredService<IMongoClient>().GetDatabase("ConaDB"));

// Register MongoDB collections
builder.Services.AddSingleton<IMongoCollection<IngredientAnalysis>>(sp => 
    sp.GetRequiredService<IMongoDatabase>().GetCollection<IngredientAnalysis>("ingredientAnalyses"));
builder.Services.AddSingleton<IMongoCollection<Recipe>>(sp => 
    sp.GetRequiredService<IMongoDatabase>().GetCollection<Recipe>("recipes"));
builder.Services.AddSingleton<IMongoCollection<CachedIngredient>>(sp => 
    sp.GetRequiredService<IMongoDatabase>().GetCollection<CachedIngredient>("cachedIngredients"));

// Register NutritionService
builder.Services.AddSingleton<NutritionService>();

// Add HttpClient
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
