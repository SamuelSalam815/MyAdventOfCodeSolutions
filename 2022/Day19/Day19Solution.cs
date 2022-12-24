using Day19;

internal class Day19Solution
{
    private static void Main()
    {
        StreamReader inputFile = new("example.txt");
        string? line;

        if ((line = inputFile.ReadLine()) is not null)
        {
            System.Console.WriteLine(Blueprint.Parse(line));
        }

    }
}