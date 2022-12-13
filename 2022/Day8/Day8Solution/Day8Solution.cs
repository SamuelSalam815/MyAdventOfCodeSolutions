using Day8Solution;

namespace Day7Solution
{
    internal class Day8Solution
    {

        static char[][] GetHeightMap(List<string> inputLines)
        {
            return inputLines
                .Select(
                    line => line
                        .ToCharArray()
                ).ToArray();
        }

        static void Main(string[] args)
        {
            // Parse into a character Array
            // Get lines from input file
            StreamReader inputFile = new("input.txt");
            List<string> lines = new();
            string? currentLine;
            while((currentLine = inputFile.ReadLine()) is not null)
            {
                lines.Add(currentLine);
            }
            // Turn lines into char array
            char[][] heightMap = GetHeightMap(lines);

            // Set up iteration over all tree positions
            int numVisibleTrees = 0;
            int bestScenicScore = int.MinValue;
            int currentScenicScore;

            // Determine the directions to consider relative to the tree position
            List<Direction> directions = new() {Direction.North,Direction.East,Direction.South,Direction.West};

            for(int rowIndex = 0; rowIndex < heightMap.Length; rowIndex++)
            {
                for(int columnIndex = 0; columnIndex < heightMap[0].Length; columnIndex++)
                {

                    TreePosition treePosition = new(rowIndex, columnIndex);
                    bool treeIsVisible = false;
                    currentScenicScore = 1;

                    foreach(Direction direction in directions)
                    {
                        if (treePosition.IsVisibleFrom(direction, heightMap, out int directionalScenicScore))
                        {
                            treeIsVisible = true;
                        }
                        currentScenicScore *= directionalScenicScore;
                    }

                    if (treeIsVisible)
                    {
                        numVisibleTrees++;
                    }

                    bestScenicScore = currentScenicScore > bestScenicScore ? currentScenicScore : bestScenicScore;

                }
            }

            Console.WriteLine($"Number of visible trees : {numVisibleTrees}");
            Console.WriteLine($"Highest Scenic Score : {bestScenicScore}");

        }
    }
}