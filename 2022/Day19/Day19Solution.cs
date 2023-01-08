using Day19.RobotConstruction;

namespace Day19;

internal static class Day19Solution
{
    private static void Main()
    {
        StreamReader inputFile = new("example.txt");
        string? line;
        long totalQualityLevel = 0;
        if ((line = inputFile.ReadLine()) is not null)
        {
            Blueprint currentBlueprint = Blueprint.Parse(line);
            System.Console.WriteLine(currentBlueprint);
            totalQualityLevel += currentBlueprint.GetQualityLevel(TimeLimitMinutes: 24);
        }
        System.Console.WriteLine(totalQualityLevel);
    }
}