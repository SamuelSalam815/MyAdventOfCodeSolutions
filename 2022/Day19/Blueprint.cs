namespace Day19;

public record struct Blueprint
(
int BluePrintId,
SingleIngredientRecipe OreRobotRecipe,
SingleIngredientRecipe ClayRobotRecipe,
TwoIngredientRecipe ObsidianRobotRecipe,
TwoIngredientRecipe GeodeRobotRecipe
)
{
        public static Blueprint Parse(string input)
        {
                string[] blueprintDefinitionParts = input.Split(": ");

                string[] bluePrintIdTextParts = blueprintDefinitionParts[0].Split(' ', 2);

                int blueprintId = int.Parse(bluePrintIdTextParts[1]);

                string[] allRecipes = blueprintDefinitionParts[1].Split(". ");

                string[] oreRobotTextParts = allRecipes[0].Split(' ');
                SingleIngredientRecipe oreRobot = new(int.Parse(oreRobotTextParts[4]));

                string[] clayRobotTextParts = allRecipes[1].Split(' ');
                SingleIngredientRecipe clayRobot = new(int.Parse(clayRobotTextParts[4]));

                string[] obsidianRobotTextParts = allRecipes[2].Split(' ');
                TwoIngredientRecipe obsidianRobot = new(
                    int.Parse(obsidianRobotTextParts[4]),
                    int.Parse(obsidianRobotTextParts[7])
                );

                string[] geodeRobotTextParts = allRecipes[3].Split(' ');
                TwoIngredientRecipe geodeRobot = new(
                    int.Parse(geodeRobotTextParts[4]),
                    int.Parse(geodeRobotTextParts[7])
                );

                return new Blueprint(
                    blueprintId,
                    oreRobot,
                    clayRobot,
                    obsidianRobot,
                    geodeRobot);

        }
}