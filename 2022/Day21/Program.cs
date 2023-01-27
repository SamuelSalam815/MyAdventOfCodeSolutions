using Day21;

internal class Program
{
    public const string HumanName = "humn";
    public const string RootName = "root";
    record MonkeyNumberJob(string MonkeyName, long NumberShouted);
    private static void Main(string[] args)
    {
        Dictionary<string, IMonkeyTreeNode> treeMonkies = new();

        StreamReader reader = new("input.txt");
        string? line;

        MonkeyOperationNode? humanParentNode = null;

        while ((line = reader.ReadLine()) is not null)
        {
            string[] parts = line.Split(": ");
            string monkeyName = parts[0];
            string jobDefinition = parts[1];

            if (long.TryParse(jobDefinition, out long numberToShout))
            {
                treeMonkies.Add(monkeyName, new MonkeyNumberNode(monkeyName, numberToShout));
            }
            else
            {
                string[] subparts = jobDefinition.Split(' ');
                string leftArgumentName = subparts[0];
                string operationSymbol = subparts[1];
                string rightArgumentName = subparts[2];

                Func<long, long, long> calculateValueOperation;
                Func<long, long, long> calculateLeftOperation;
                Func<long, long, long> calculateRightOperation;

                switch (operationSymbol)
                {
                    case "+":
                        calculateValueOperation = (left, right) => left + right;
                        calculateLeftOperation = (result, right) => result - right;
                        calculateRightOperation = (result, left) => result - left;
                        break;
                    case "-":
                        calculateValueOperation = (left, right) => left - right;
                        calculateLeftOperation = (result, right) => result + right;
                        calculateRightOperation = (result, left) => left - result;
                        break;
                    case "*":
                        calculateValueOperation = (left, right) => left * right;
                        calculateLeftOperation = (result, right) => result / right;
                        calculateRightOperation = (result, left) => result / left;
                        break;
                    case "/":
                        calculateValueOperation = (left, right) => left / right;
                        calculateLeftOperation = (result, right) => result * right;
                        calculateRightOperation = (result, left) => left / result;
                        break;
                    default:
                        throw new Exception($"Unsupported operation symbol {operationSymbol}");
                }

                MonkeyOperationNode latestNode = new(
                    name: monkeyName,
                    leftChildName: leftArgumentName,
                    rightChildName: rightArgumentName,
                    nodeMap: treeMonkies,
                    calculateValueOperation,
                    calculateLeftOperation,
                    calculateRightOperation);

                if (leftArgumentName.Equals(HumanName) || rightArgumentName.Equals(HumanName))
                {
                    if (humanParentNode is not null)
                    {
                        throw new Exception("Encountered multiple parents for the human node. Error");
                    }
                    humanParentNode = latestNode;
                }

                treeMonkies.Add(monkeyName, latestNode);
            }
        }

        Console.WriteLine(treeMonkies[RootName].GetValue());

        if (humanParentNode is null)
        {
            throw new Exception("Human node has no parent node, its value cannot be inferred");
        }

        if (humanParentNode.LeftChild.Name.Equals(HumanName))
        {
            Console.WriteLine(humanParentNode.CalculateLeftChildOperation(humanParentNode.InferValue(), humanParentNode.RightChild.GetValue()));
        }
        else
        {
            Console.WriteLine(humanParentNode.CalculateRightChildOperation(humanParentNode.InferValue(), humanParentNode.LeftChild.GetValue()));
        }
    }

}