using MongoDB.Bson.Serialization.Attributes;

namespace ConaApi.Models;

public class Recipe
{
    [BsonId]
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public List<string> Ingredients { get; set; }
    public string Instructions { get; set; }
}
