using System.Net.Http.Json;
using MongoDB.Driver;
using ConaApi.Models;

namespace ConaApi.Services;

public class NutritionService
{
    private readonly IMongoCollection<CachedIngredient> _cacheCollection;
    private readonly HttpClient _httpClient;
    private readonly string _usdaApiKey;
    private const int CACHE_DAYS = 7;

    public NutritionService(
        IMongoCollection<CachedIngredient> cacheCollection,
        IConfiguration configuration)
    {
        _cacheCollection = cacheCollection;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.nal.usda.gov/fdc/v1/")
        };
        _usdaApiKey = configuration["UsdaApiKey"] 
            ?? throw new ArgumentNullException("USDA API key not found in configuration");
    }

    public async Task<Dictionary<string, double>> GetNutrientsForIngredient(string ingredient)
    {
        // Check cache first
        var cached = await _cacheCollection
            .Find(c => c.Name.ToLower() == ingredient.ToLower() && c.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync();

        if (cached != null)
        {
            return cached.Nutrients;
        }

        // If not in cache, fetch from USDA API
        var nutrients = await FetchFromUsda(ingredient);
        
        // Cache the results
        await CacheIngredient(ingredient, nutrients);

        return nutrients;
    }

    private async Task<Dictionary<string, double>> FetchFromUsda(string ingredient)
    {
        try
        {
            // Search for the ingredient
            var searchResponse = await _httpClient.GetFromJsonAsync<UsdaSearchResponse>(
                $"foods/search?api_key={_usdaApiKey}&query={Uri.EscapeDataString(ingredient)}&pageSize=1");

            if (searchResponse?.Foods == null || !searchResponse.Foods.Any())
            {
                return new Dictionary<string, double>();
            }

            // Get detailed nutrients for the first result
            var foodDetails = await _httpClient.GetFromJsonAsync<UsdaFoodDetails>(
                $"food/{searchResponse.Foods[0].FdcId}?api_key={_usdaApiKey}");

            if (foodDetails?.FoodNutrients == null)
            {
                return new Dictionary<string, double>();
            }

            // Map nutrients to our format
            return MapNutrients(foodDetails.FoodNutrients);
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"Error fetching nutrition data: {ex.Message}");
            return new Dictionary<string, double>();
        }
    }

    private async Task CacheIngredient(string ingredient, Dictionary<string, double> nutrients)
    {
        var cachedIngredient = new CachedIngredient
        {
            Id = Guid.NewGuid().ToString(),
            Name = ingredient,
            Nutrients = nutrients,
            CachedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(CACHE_DAYS)
        };

        await _cacheCollection.InsertOneAsync(cachedIngredient);
    }

    private Dictionary<string, double> MapNutrients(List<UsdaNutrient> nutrients)
    {
        var mappedNutrients = new Dictionary<string, double>();
        var nutrientMappings = new Dictionary<int, string>
        {
            { 203, "Protein" },
            { 204, "Fat" },
            { 205, "Carbohydrates" },
            { 208, "Calories" },
            { 269, "Sugars" },
            { 291, "Fiber" },
            { 301, "Calcium" },
            { 303, "Iron" },
            { 306, "Potassium" },
            { 307, "Sodium" },
            { 401, "Vitamin C" },
            { 404, "Thiamin" },
            { 405, "Riboflavin" },
            { 406, "Niacin" },
        };

        foreach (var nutrient in nutrients)
        {
            if (nutrientMappings.TryGetValue(nutrient.NutrientId, out string? name))
            {
                mappedNutrients[name] = nutrient.Amount;
            }
        }

        return mappedNutrients;
    }
}

// USDA API response models
public class UsdaSearchResponse
{
    public List<UsdaFood> Foods { get; set; } = new();
}

public class UsdaFood
{
    public int FdcId { get; set; }
}

public class UsdaFoodDetails
{
    public List<UsdaNutrient> FoodNutrients { get; set; } = new();
}

public class UsdaNutrient
{
    public int NutrientId { get; set; }
    public double Amount { get; set; }
}
