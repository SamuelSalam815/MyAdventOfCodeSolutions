internal class Day12Solution
{
        private static int MinimumStepsFromAToB(List<List<char>> grid, int startingX, int startingY, int endingX, int endingY)
        {
                int moveCount = 0;
                HashSet<(int x, int y)> alreadySeenSet = new();
                Queue<(int x, int y)> positionsToVisit = new();
                positionsToVisit.Enqueue((startingX, startingY));

                int numPositionsToVisitAtSameBreadth = positionsToVisit.Count;
                while (numPositionsToVisitAtSameBreadth > 0)
                {
                        (int x, int y) = positionsToVisit.Dequeue();

                        // Check for finishing square
                        if ((x, y) == (endingX, endingY))
                        {
                                return moveCount;
                        }

                        (int, int)[] candidatePositions = new (int, int)[]
                        {
                                (x-1,y),
                                (x+1,y),
                                (x,y-1),
                                (x,y+1)
                        };


                        foreach ((int newX, int newY) in candidatePositions)
                        {
                                // Reject positions already seen
                                if (alreadySeenSet.Contains((newX, newY)))
                                {
                                        continue;
                                }

                                // Reject coords out of bounds
                                if (newY < 0 || newY >= grid.Count)
                                {
                                        continue;
                                }
                                if (newX < 0 || newX >= grid[0].Count)
                                {
                                        continue;
                                }

                                // Reject changes in elevation too great
                                if (grid[newY][newX] - grid[y][x] > 1)
                                {
                                        continue;
                                }

                                positionsToVisit.Enqueue((newX, newY));
                                alreadySeenSet.Add((newX, newY));
                        }

                        numPositionsToVisitAtSameBreadth--;
                        if (numPositionsToVisitAtSameBreadth == 0)
                        {
                                moveCount++;
                                numPositionsToVisitAtSameBreadth = positionsToVisit.Count;
                        }
                }
                return int.MaxValue;
        }

        private static void Main(string[] args)
        {
                const bool isPart1 = false;
                StreamReader input = new("input.txt");
                string? line;
                List<List<char>> grid = new();
                while((line = input.ReadLine()) is not null)
                {
                        grid.Add(line.ToList());
                }

                string GridToString()
                {
                        return string.Join('\n', grid.Select(chars=> new string(chars.ToArray())));
                }

                int startingX = -1;
                int startingY = -1;
                int endingX = -1;
                int endingY = -1;

                for(int rowIndex = 0; rowIndex < grid.Count; rowIndex++)
                {
                        for(int colIndex = 0; colIndex < grid[0].Count; colIndex++)
                        {
                                if (grid[rowIndex][colIndex] == 'S')
                                {
                                        startingX = colIndex;
                                        startingY = rowIndex;
                                        grid[rowIndex][colIndex] = 'a';
                                }
                                if (grid[rowIndex][colIndex] == 'E')
                                {
                                        endingX = colIndex;
                                        endingY = rowIndex;
                                        grid[rowIndex][colIndex] = 'z';
                                }
                        }
                }

                List<(int, int)> startingPoints = new() { };
                if (isPart1)
                {
                        startingPoints.Add((startingX, startingY));
                }
                else
                {
                        for(int y = 0; y < grid.Count; y++)
                        {
                                for(int x = 0; x < grid[0].Count; x++)
                                {
                                        if (grid[y][x] == 'a')
                                        {
                                                startingPoints.Add((x, y));
                                        }
                                }
                        }
                }

                int minSteps = startingPoints
                        .Select(point => MinimumStepsFromAToB(grid, point.Item1, point.Item2, endingX, endingY))
                        .Min();

                Console.WriteLine(minSteps);
        }
}