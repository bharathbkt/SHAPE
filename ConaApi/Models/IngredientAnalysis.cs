using MongoDB.Bson.Serialization.Attributes;

namespace ConaApi.Models;

public class IngredientAnalysis 
{
    [BsonId]
    public string Id { get; set; }
    public string UserId { get; set; }
    public List<string> Ingredients { get; set; }
    public Dictionary<string, double> Nutrients { get; set; } // e.g., "VitaminC": 12.5
    public List<string> CookingRecommendations { get; set; }
}
