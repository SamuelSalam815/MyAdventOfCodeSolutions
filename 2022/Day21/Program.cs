using Day21;
using Day21.TreeApproach;

internal class Program
{
    record MonkeyNumberJob(string MonkeyName, long NumberShouted);
    private static void Main(string[] args)
    {
        MonkeyJobCoordinator coordinator = new();
        List<MonkeyNumberJob> numberMonkies = new();
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
                numberMonkies.Add(new(monkeyName, numberToShout));
                treeMonkies.Add(monkeyName, new MonkeyNumberNode(numberToShout));
            }
            else
            {
                string[] subparts = jobDefinition.Split(' ');
                string dependency1 = subparts[0];
                string dependency2 = subparts[2];
                if(!TryParseOperationType(subparts[1], out OperationType operationType))
                {
                    throw new Exception($"Unsupported operation '{subparts[1]}'");
                }
                _ = new MonkeyMathJob(monkeyName, dependency1, dependency2, operationType, coordinator);

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

        foreach(var independentMonkey in numberMonkies)
        {
            coordinator.TriggerMonkeyShout(independentMonkey.MonkeyName, independentMonkey.NumberShouted);
        }

        Console.WriteLine(coordinator.RootMonkeyNumber);
        Console.WriteLine(treeMonkies["root"].GetValue());
    }

    public static bool TryParseOperationType(string s, out OperationType operationType)
    {
        operationType = default;
        switch (s)
        {
            case "+":
                operationType = OperationType.Add;
                return true;
            case "-":
                operationType = OperationType.Subtract;
                return true;
            case "*":
                operationType = OperationType.Multiply;
                return true;
            case "/":
                operationType = OperationType.Divide;
                return true;
            default:
                return false;
        }
    }
}