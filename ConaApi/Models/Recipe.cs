using MongoDB.Bson.Serialization.Attributes;

namespace ConaApi.Models;

public class Recipe
{
    [BsonId]
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }  // Changed from Name to Title to match app
    public string Description { get; set; }  // Added to match app
    public List<string> Ingredients { get; set; }
    public string Instructions { get; set; }
}
