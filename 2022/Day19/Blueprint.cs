using System.Collections.ObjectModel;
using Day19.Resources;

namespace Day19;

public record struct Blueprint
(
        int BluePrintId,
        ReadOnlyDictionary<RobotType, List<Ingredient>> RobotRecipes
)
{
        public static Blueprint Parse(string input)
        {
                string[] blueprintDefinitionParts = input.Split(": ");
                string[] bluePrintIdTextParts = blueprintDefinitionParts[0].Split(' ', 2);
                int blueprintId = int.Parse(bluePrintIdTextParts[1]);

                Dictionary<RobotType,List<Ingredient>> recipes = new();
                string[][] unparsedRecipes = blueprintDefinitionParts[1]
                .Split(". ")
                .Select(recipeText => recipeText.Split(' ').ToArray())
                .ToArray();
                
                recipes.Add(RobotType.Ore, new(){new(ResourceIdentifier.Ore, int.Parse(unparsedRecipes[0][4]))});
                recipes.Add(RobotType.Clay, new(){new(ResourceIdentifier.Ore, int.Parse(unparsedRecipes[1][4]))});
                recipes.Add(RobotType.Obsidian, new(){
                        new(ResourceIdentifier.Ore, int.Parse(unparsedRecipes[2][4])),
                        new(ResourceIdentifier.Clay, int.Parse(unparsedRecipes[2][7])),
                        });
                recipes.Add(RobotType.Obsidian, new(){
                        new(ResourceIdentifier.Ore, int.Parse(unparsedRecipes[3][4])),
                        new(ResourceIdentifier.Obsidian, int.Parse(unparsedRecipes[3][7])),
                        });

                return new Blueprint(
                    blueprintId,
                    new ReadOnlyDictionary<RobotType, List<Ingredient>>(recipes)
                );

        }
}