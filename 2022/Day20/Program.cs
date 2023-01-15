namespace Day20;

internal class Program
{
    private static List<long> parseInput(string filepath)
    {
        StreamReader reader = new(filepath);
        List<long> inputList = new();
        string? line;
        while((line = reader.ReadLine()) is not null )
        {
            inputList.Add(long.Parse(line));
        }
        return inputList;
    }

    private static List<CircularListNode> GetOriginalNodeOrder(CircularListNode initialNode)
    {
        List<CircularListNode> originalNodeOrder = new();
        CircularListNode currentNode = initialNode;
        do
        {
            originalNodeOrder.Add(currentNode);
            currentNode = currentNode.NextNode;
        }
        while (currentNode != initialNode);
        return originalNodeOrder;
    }

    private static void MoveNode(CircularListNode targetNode, long stepsToTake)
    {
        while(stepsToTake != 0 )
        {
            CircularListNode originalPreviousNode = targetNode.PreviousNode;
            CircularListNode originalNextNode = targetNode.NextNode;
            CircularListNode newPreviousNode;
            CircularListNode newNextNode;

            if (stepsToTake > 0)
            {
                newPreviousNode = targetNode.NextNode;
                newNextNode = targetNode.NextNode.NextNode;
            }
            else
            {
                newPreviousNode = targetNode.PreviousNode.PreviousNode;
                newNextNode = targetNode.PreviousNode;
            }

            originalPreviousNode.NextNode = originalNextNode;
            originalNextNode.PreviousNode = originalPreviousNode;

            newPreviousNode.NextNode = targetNode;
            newNextNode.PreviousNode = targetNode;

            targetNode.PreviousNode = newPreviousNode;
            targetNode.NextNode = newNextNode;

            stepsToTake += stepsToTake > 0 ? -1 : 1;
        }

    }

    private static long GetValueAtOffset(CircularListNode targetNode, uint offset)
    {
        for(int stepsTaken = 0; stepsTaken < offset; stepsTaken++)
        {
            targetNode = targetNode.NextNode;
        }

        return targetNode.Value;
    }

    private static void Main(string[] args)
    {
        const int decryptionKey = 811589153;
        const int roundsOfMixing = 10;

        CircularListNode firstNodeParsed = new(parseInput("input.txt"));
        List<CircularListNode> originalNodeOrder = GetOriginalNodeOrder(firstNodeParsed);

        foreach(var node in originalNodeOrder)
        {
            node.Value *= decryptionKey;
        }
        
        List<CircularListNode> zeroNodes = originalNodeOrder.Where(x=>x.Value == 0).ToList();

        if(zeroNodes.Count != 1)
        {
            throw new Exception($"Exactly 1 zero node is required. Found {zeroNodes.Count}");
        }

        CircularListNode zeroNode = zeroNodes[0];

        //Console.WriteLine("Initial arrangement:");
        //Console.WriteLine(originalNodeOrder[0].GetListAsString());

        for(int i = 1; i <= roundsOfMixing; i++)
        {
            foreach(CircularListNode node in originalNodeOrder)
            {
                long stepsToTake = (Math.Abs(node.Value) ) % (originalNodeOrder.Count-1) * (node.Value > 0 ? 1 : -1 );
                MoveNode(node, stepsToTake);
            }
            //Console.WriteLine();
            //Console.WriteLine($"After {i} round{(i>1?"s":"")} of mixing:");
            //Console.WriteLine(zeroNode.GetListAsString());
        }

        long val1 = GetValueAtOffset(zeroNode, 1000);
        long val2 = GetValueAtOffset(zeroNode, 2000);
        long val3 = GetValueAtOffset(zeroNode, 3000);

        //Console.WriteLine();
        //Console.WriteLine($"The 1000th number after 0 is {val1}");
        //Console.WriteLine($"The 2000th number after 0 is {val2}");
        //Console.WriteLine($"The 3000th number after 0 is {val3}");
        //Console.WriteLine($"The sum of the three numbers that form the grove coordinates is {val1 + val2 + val3}");

        Console.WriteLine(val1 + val2 + val3);
    }
}
