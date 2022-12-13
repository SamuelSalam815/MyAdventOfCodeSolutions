using System.Runtime.CompilerServices;

public class Day5Solution
{
    public struct Instruction
    {
        public int quantityToMove;
        public int sourceStack;
        public int targetStack;

        public Instruction(int quantity, int source, int target)
        {
            quantityToMove = quantity;
            sourceStack = source;
            targetStack = target;
        }
    }
    public static Instruction ParseInstruction(string line)
    {
        string[] fragments = line.Split(' ');
        _ = int.TryParse(fragments[1],out int quantityToMove);
        _ = int.TryParse(fragments[3],out int source);
        _ = int.TryParse(fragments[5],out int target);
        return new Instruction(quantityToMove, source, target);
    }
    public static char[][] ParseStartingStack(string[] lines)
    {
        int stackCount = ((lines[0].Length - 1) / 4) + 1;
        
        List<char>[] stacks = new List<char>[stackCount];
        for(int i = 0; i < stackCount; i++)
        {
            stacks[i] = new();
        }


        foreach(string line in lines)
        {
            if (line[1] == '1') break;
            for(int i = 1; i < line.Length; i += 4)
            {
                if ('A' <= line[i] && line[i] <= 'Z')
                {
                    stacks[(i-1)/4].Add(line[i]);
                }
            }
        }

        char[][] finalResult = new char[stackCount][];
        for(int i = 0; i < stackCount; i++)
        {
            finalResult[i] = stacks[i].ToArray();
        }

        return finalResult;
    }
    private static void Main(string[] args)
    {
        using StreamReader inputFile = new("input.txt");
        List<string> startingStackUnparsed = new();
        string? line;
        
        while (true)
        {
            line = inputFile.ReadLine();
            if (line[1] == '1') break;
            startingStackUnparsed.Add(line);
        }

        inputFile.ReadLine(); // Skip the blank line

        char[][] startingStack = ParseStartingStack(startingStackUnparsed.ToArray());
        List<Stack<char>> charsInStack = new();
        foreach (char[] stack in startingStack)
        {
            charsInStack.Add(new Stack<char>(stack.Reverse()));
        }

        while( (line = inputFile.ReadLine()) is not null)
        {
            Instruction currentInstruction = ParseInstruction(line);
            List<char> buffer = new();
            while(currentInstruction.quantityToMove > 0)
            {
                Stack<char> sourceStack = charsInStack[currentInstruction.sourceStack - 1];
                buffer.Add(sourceStack.Pop());
                currentInstruction.quantityToMove--;
            }
            foreach(char c in buffer.Reverse<char>())
            {
                Stack<char> targetStack = charsInStack[currentInstruction.targetStack - 1];
                targetStack.Push(c);
            }
        }

        string finalResult = "";
        foreach(Stack<char> stack in charsInStack)
        {
            finalResult += stack.Pop();
        }

        Console.WriteLine(finalResult);
    }
}