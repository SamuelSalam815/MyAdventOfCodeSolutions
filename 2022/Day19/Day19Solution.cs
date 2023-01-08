using Day19.RobotConstruction;

namespace Day19;

internal static class Day19Solution
{
    private static void Main()
    {
        StreamReader inputFile = new("input.txt");
        string? line;
        long totalQualityLevel = 0;
        while ((line = inputFile.ReadLine()) is not null)
        {
            Blueprint currentBlueprint = Blueprint.Parse(line);
            totalQualityLevel += currentBlueprint.GetQualityLevel(TimeLimitMinutes: 24);
        }
        System.Console.WriteLine(totalQualityLevel);
        System.Console.WriteLine("Fin.");
    }
}