using MongoDB.Bson.Serialization.Attributes;

namespace ConaApi.Models;

public class CachedIngredient
{
    [BsonId]
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, double> Nutrients { get; set; }
    public DateTime CachedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}
