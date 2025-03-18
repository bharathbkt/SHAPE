namespace ConaApi.Services;

public static class NutrientOptimizer
{
    // Dictionary of ingredient pairs that enhance nutrient absorption
    private static readonly Dictionary<(string, string), string> IngredientSynergies = new()
    {
        { ("turmeric", "black pepper"), "Add black pepper to turmeric to boost curcumin absorption by up to 2000%" },
        { ("tomato", "olive oil"), "Combine tomatoes with olive oil to enhance lycopene absorption" },
        { ("green tea", "lemon"), "Add lemon to green tea to increase catechin absorption" },
        { ("spinach", "lemon"), "Add citrus to spinach to enhance iron absorption" },
        { ("kale", "avocado"), "Pair kale with healthy fats like avocado to boost vitamin absorption" },
        { ("carrots", "olive oil"), "Combine carrots with healthy fats to improve vitamin A absorption" }
    };

    // Dictionary of nutrient pairs that work together
    private static readonly Dictionary<(string, string), string> NutrientSynergies = new()
    {
        { ("Vitamin D", "Calcium"), "Vitamin D helps calcium absorption" },
        { ("Vitamin C", "Iron"), "Vitamin C enhances iron absorption" },
        { ("Vitamin B12", "Folate"), "B12 and folate work together for DNA synthesis" },
        { ("Zinc", "Vitamin A"), "Zinc helps convert vitamin A to its active form" },
        { ("Vitamin E", "Vitamin C"), "Vitamins E and C work together as antioxidants" }
    };

    // Common ingredient combinations for specific health goals
    private static readonly Dictionary<string, List<string>> HealthGoalCombinations = new()
    {
        { 
            "immune_boost", 
            new List<string> { 
                "Combine citrus fruits with zinc-rich foods like pumpkin seeds",
                "Pair garlic with vitamin C-rich vegetables"
            }
        },
        { 
            "heart_health", 
            new List<string> { 
                "Combine omega-3 rich foods with anti-inflammatory spices",
                "Pair leafy greens with healthy fats for nutrient absorption"
            }
        },
        { 
            "energy_boost", 
            new List<string> { 
                "Combine complex carbs with protein for sustained energy",
                "Pair iron-rich foods with vitamin C for better absorption"
            }
        }
    };

    public static List<string> GetCookingRecommendations(List<string> ingredients)
    {
        var recommendations = new List<string>();

        // Check for ingredient synergies
        for (int i = 0; i < ingredients.Count; i++)
        {
            for (int j = i + 1; j < ingredients.Count; j++)
            {
                var ingredient1 = ingredients[i].ToLower();
                var ingredient2 = ingredients[j].ToLower();

                // Check both orderings of the ingredient pair
                if (IngredientSynergies.TryGetValue((ingredient1, ingredient2), out string? synergy1))
                {
                    recommendations.Add(synergy1);
                }
                else if (IngredientSynergies.TryGetValue((ingredient2, ingredient1), out string? synergy2))
                {
                    recommendations.Add(synergy2);
                }
            }
        }

        // Add missing beneficial combinations
        foreach (var (pair, recommendation) in IngredientSynergies)
        {
            if (ingredients.Contains(pair.Item1, StringComparer.OrdinalIgnoreCase) &&
                !ingredients.Contains(pair.Item2, StringComparer.OrdinalIgnoreCase))
            {
                recommendations.Add($"Consider adding {pair.Item2} to your recipe: {recommendation}");
            }
        }

        // Add general health optimization tips
        recommendations.AddRange(new[]
        {
            "Consider cooking methods that preserve nutrients:",
            "- Steam vegetables instead of boiling to retain water-soluble vitamins",
            "- Use low to medium heat when cooking with oils to prevent nutrient degradation",
            "- Chop garlic and let it rest for 10 minutes before cooking to maximize beneficial compounds"
        });

        return recommendations;
    }

    public static List<string> GetNutrientOptimizationTips(Dictionary<string, double> nutrients)
    {
        var tips = new List<string>();

        // Check for nutrient synergies
        foreach (var (pair, tip) in NutrientSynergies)
        {
            if (nutrients.ContainsKey(pair.Item1) && nutrients.ContainsKey(pair.Item2))
            {
                tips.Add($"Good combination: {tip}");
            }
            else if (nutrients.ContainsKey(pair.Item1) && !nutrients.ContainsKey(pair.Item2))
            {
                tips.Add($"Consider adding foods rich in {pair.Item2}: {tip}");
            }
        }

        // Add tips based on nutrient levels
        if (nutrients.TryGetValue("Iron", out double iron) && iron < 18) // Example threshold
        {
            tips.Add("To boost iron absorption, pair iron-rich foods with vitamin C sources");
        }

        if (nutrients.TryGetValue("Calcium", out double calcium) && calcium < 1000) // Example threshold
        {
            tips.Add("For better calcium absorption, ensure adequate vitamin D intake");
        }

        return tips;
    }

    public static List<string> GetHealthGoalRecommendations(string healthGoal)
    {
        if (HealthGoalCombinations.TryGetValue(healthGoal.ToLower(), out var recommendations))
        {
            return recommendations;
        }

        return new List<string>
        {
            "Focus on a balanced combination of proteins, healthy fats, and complex carbohydrates",
            "Include a variety of colorful vegetables and fruits for diverse nutrient intake"
        };
    }
}
