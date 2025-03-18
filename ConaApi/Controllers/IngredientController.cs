using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using ConaApi.Models;
using ConaApi.Services;

namespace ConaApi.Controllers;

[Authorize]
[ApiController]
[Route("api")]
public class IngredientController : ControllerBase
{
    private readonly IMongoCollection<IngredientAnalysis> _analysisCollection;
    private readonly IMongoCollection<Recipe> _recipeCollection;
    private readonly NutritionService _nutritionService;

    public IngredientController(
        IMongoCollection<IngredientAnalysis> analysisCollection,
        IMongoCollection<Recipe> recipeCollection,
        NutritionService nutritionService)
    {
        _analysisCollection = analysisCollection;
        _recipeCollection = recipeCollection;
        _nutritionService = nutritionService;
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<IngredientAnalysis>> AnalyzeIngredients([FromBody] AnalyzeRequest request)
    {
        if (request.Ingredients == null || !request.Ingredients.Any())
            return BadRequest("No ingredients provided");

        var userId = User.FindFirst("email")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        // Get nutritional data for each ingredient
        var nutrientTasks = request.Ingredients.Select(i => _nutritionService.GetNutrientsForIngredient(i));
        var nutrientResults = await Task.WhenAll(nutrientTasks);

        // Combine nutrients from all ingredients
        var combinedNutrients = new Dictionary<string, double>();
        foreach (var nutrients in nutrientResults)
        {
            foreach (var (nutrient, amount) in nutrients)
            {
                if (combinedNutrients.ContainsKey(nutrient))
                    combinedNutrients[nutrient] += amount;
                else
                    combinedNutrients[nutrient] = amount;
            }
        }

        // Generate comprehensive recommendations
        var recommendations = new List<string>();
        
        // Get ingredient interaction recommendations
        recommendations.AddRange(NutrientOptimizer.GetCookingRecommendations(request.Ingredients));
        
        // Get nutrient optimization tips
        recommendations.AddRange(NutrientOptimizer.GetNutrientOptimizationTips(combinedNutrients));
        
        // Add health goal specific recommendations if provided
        if (!string.IsNullOrEmpty(request.HealthGoal))
        {
            recommendations.AddRange(NutrientOptimizer.GetHealthGoalRecommendations(request.HealthGoal));
        }

        var analysis = new IngredientAnalysis
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            Ingredients = request.Ingredients,
            Nutrients = combinedNutrients,
            CookingRecommendations = recommendations
        };

        await _analysisCollection.InsertOneAsync(analysis);
        return Ok(analysis);
    }

    [HttpGet("recipes")]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
    {
        var userId = User.FindFirst("email")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var recipes = await _recipeCollection
            .Find(r => r.UserId == userId)
            .ToListAsync();

        return Ok(recipes);
    }

    [HttpGet("recipes/{id}")]
    public async Task<ActionResult<Recipe>> GetRecipeById(string id)
    {
        var userId = User.FindFirst("email")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var recipe = await _recipeCollection
            .Find(r => r.Id == id && r.UserId == userId)
            .FirstOrDefaultAsync();

        if (recipe == null)
            return NotFound();

        return Ok(recipe);
    }

    [HttpPost("recipes")]
    public async Task<ActionResult<Recipe>> SaveRecipe([FromBody] Recipe recipe)
    {
        var userId = User.FindFirst("email")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        if (string.IsNullOrEmpty(recipe.Title))
            return BadRequest("Recipe title is required");

        recipe.Id = Guid.NewGuid().ToString();
        recipe.UserId = userId;

        // Generate description if not provided
        if (string.IsNullOrEmpty(recipe.Description))
        {
            recipe.Description = $"A delicious recipe with {string.Join(", ", recipe.Ingredients)}";
        }

        await _recipeCollection.InsertOneAsync(recipe);
        return Ok(recipe);
    }
}

public class AnalyzeRequest
{
    public List<string> Ingredients { get; set; }
    public string? HealthGoal { get; set; }
}
