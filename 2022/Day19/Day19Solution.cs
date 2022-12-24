internal class Day19Solution
{
    private static void Main(string[] args)
    {
        StreamReader inputFile = new("example.txt");
        string? line;

        if ((line = inputFile.ReadLine()) is not null)
        {
            System.Console.WriteLine(Blueprint.Parse(line));
        }

    }
}