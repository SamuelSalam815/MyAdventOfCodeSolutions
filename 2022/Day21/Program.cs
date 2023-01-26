using Day21;

internal class Program
{
    record MonkeyNumberJob(string MonkeyName, long NumberShouted);
    private static void Main(string[] args)
    {
        Dictionary<string, IMonkeyTreeNode> treeMonkies = new();

        StreamReader reader = new("input.txt");
        string? line;
        
        while((line = reader.ReadLine()) is not null)
        {
            string[] parts = line.Split(": ");
            string monkeyName = parts[0];
            string jobDefinition = parts[1];

            if(long.TryParse(jobDefinition, out long numberToShout))
            {
                treeMonkies.Add(monkeyName, new MonkeyNumberNode(numberToShout));
            }
            else
            {
                string[] subparts = jobDefinition.Split(' ');
                string dependency1 = subparts[0];
                string dependency2 = subparts[2];
                Func<long, long, long> operation = subparts[1] switch
                {
                    "+" => (a,b) => a + b,
                    "-" => (a,b) => a - b,
                    "*" => (a,b) => a * b,
                    "/" => (a,b) => a / b,
                    _ => throw new Exception("Unsupported operation type")
                };

                treeMonkies.Add(monkeyName, new MonkeyOperationNode(dependency1, dependency2, operation, treeMonkies));
            }
        }

        Console.WriteLine(treeMonkies["root"].GetValue());
    }

}