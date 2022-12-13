using Day9;

internal class Day9Solution
{
    

    private static void Main(string[] args)
    {
        StreamReader inputFile = new("puzzle input.txt");
        string? line;
        KnotPositionSimulator simulator = new(numOfKnots: 10);

        while((line = inputFile.ReadLine()) is not null)
        {
            string[] commandParts = line.Split(' ', 2);
            int numRepetitions = int.Parse(commandParts[1]);

            simulator.ExecuteHeadMovement(commandParts[0], numRepetitions);
        }

        Console.WriteLine($"Number of unique locations the tail has visited : {simulator.GetNumberOfDistinctTailPositions()}");
    }
}