using System.Collections.ObjectModel;
using Day19.Resources;

namespace Day19.RobotConstruction;

public record struct Blueprint
(
        int BlueprintId,
        ReadOnlyDictionary<RobotType, List<Ingredient>> RobotRecipes
)
{
    public static Blueprint Parse(string input)
    {
        string[] blueprintDefinitionParts = input.Split(": ");
        string[] bluePrintIdTextParts = blueprintDefinitionParts[0].Split(' ', 2);
        int blueprintId = int.Parse(bluePrintIdTextParts[1]);

        Dictionary<RobotType, List<Ingredient>> recipes = new();
        string[][] unparsedRecipes = blueprintDefinitionParts[1]
        .Split(". ")
        .Select(recipeText => recipeText.Split(' ').ToArray())
        .ToArray();

        recipes.Add(RobotType.Ore, new() { new(ResourceType.Ore, int.Parse(unparsedRecipes[0][4])) });
        recipes.Add(RobotType.Clay, new() { new(ResourceType.Ore, int.Parse(unparsedRecipes[1][4])) });
        recipes.Add(RobotType.Obsidian, new(){
                new(ResourceType.Ore, int.Parse(unparsedRecipes[2][4])),
                new(ResourceType.Clay, int.Parse(unparsedRecipes[2][7])),
        });
        recipes.Add(RobotType.Geode, new(){
                new(ResourceType.Ore, int.Parse(unparsedRecipes[3][4])),
                new(ResourceType.Obsidian, int.Parse(unparsedRecipes[3][7])),
        });

        return new Blueprint(
            blueprintId,
            new ReadOnlyDictionary<RobotType, List<Ingredient>>(recipes)
        );
    }

    private static string RobotRecipeToString(RobotType robotType, List<Ingredient> recipe)
    {
        string recipeText = string.Join(", ", recipe.Select(r => $"{r.RequiredQuantity} {r.ResourceType}"));
        return $"Robot type {robotType} requires {recipeText} to be constructed";
    }

    public override string ToString()
    {
        return $"BlueprintId : {BlueprintId}\n{string.Join('\n', RobotRecipes.Select(x => RobotRecipeToString(x.Key, x.Value)))}";
    }

    internal long GetQualityLevel(int TimeLimitMinutes)
    {
        return RobotRecipes.Count + TimeLimitMinutes;
    }
}