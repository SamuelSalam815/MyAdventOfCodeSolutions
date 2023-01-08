using Day19.RobotConstruction;

namespace Day19;

internal static class Day19Solution
{
    private static void Main()
    {
        StreamReader inputFile = new("input.txt");
        string? line;
        bool isPart1 = false;
        long result;
        if (isPart1)
        {
            result = 0;
            while ((line = inputFile.ReadLine()) is not null)
            {
                Blueprint currentBlueprint = Blueprint.Parse(line);
                result += currentBlueprint.GetQualityLevel(TimeLimitMinutes: 24);
            }
        }
        else
        {
            result = 1;
            for (
                int linesRead = 0;
                (line = inputFile.ReadLine()) is not null && linesRead < 3;
                linesRead++
            )
            {
                Blueprint currentBlueprint = Blueprint.Parse(line);
                result *= currentBlueprint.GetMaxGeodesProduced(TimeLimitMinutes: 32);
            }
        }
        System.Console.WriteLine(result);
        System.Console.WriteLine("Fin.");
    }
}
