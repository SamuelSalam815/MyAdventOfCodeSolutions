using Day6;

namespace Day6Solution
{
    public class Day6Solution
    {

        static void Main(string[] args)
        {
            PuzzleDirectoryMaker directoryMaker = new();
            using StreamReader inputFile = new("input.txt");
            

            while(!inputFile.EndOfStream)
            {
                string? commandExecutedLine = inputFile.ReadLine();
                if(commandExecutedLine.StartsWith("$ cd"))
                {
                    directoryMaker.ChangeDirectory(commandExecutedLine);
                } 
                else if(commandExecutedLine.StartsWith("$ ls"))
                {
                    List<string> currentArguments = new();
                    currentArguments.Add(commandExecutedLine);
                    while (inputFile.Peek() != '$' && inputFile.Peek() != -1)
                    {
                        currentArguments.Add(inputFile.ReadLine());
                    }
                    directoryMaker.ListCommand(currentArguments);
                }
                else
                {
                    throw new Exception("Unknown command");
                }
            }


            List<PuzzleDirectory> directories = directoryMaker.GetAllDirectories();

            long part1Answer = directories
                .Select(dir => dir.DirectorySizeBytes)
                .Where(size => size <= 100_000L)
                .Sum();

            Console.WriteLine($"Part 1 answer : {part1Answer}");

            const long diskCapacity = 70_000_000L;
            const long neededSpace = 30_000_000L;

            long diskUsed = directories.Where(dir => dir.DirectoryName.Equals(@"/")).First().DirectorySizeBytes;
            long freeSpace = diskCapacity - diskUsed;

            long part2Answer = directories
                .Select(dir => dir.DirectorySizeBytes)
                .Where(size => size + freeSpace >= neededSpace)
                .Min();

            Console.WriteLine($"Part 2 answer : {part2Answer}");
        }
    }
}