using System.Diagnostics.SymbolStore;

namespace Day14;

internal class Day14Solution
{
        public record struct Position(int X, int Y);

        static HashSet<Position> ConstructMap(List<RockStrucure> allRockStructures)
        {
                HashSet<Position> positionsWithRocks = new();

                foreach (RockStrucure strucure in allRockStructures)
                {
                        foreach (Position rockPosition in strucure.GetAllRockPositions())
                        {
                                positionsWithRocks.Add(rockPosition);
                        }
                }

                return positionsWithRocks;
        }

        // Returns true if sand settles and rockMap is updated (treat settled sand as a rock).
        // Returns false if it drops into the void
        static bool TrySettleSand(int sandX, int sandY, HashSet<Position> rockMap, int floorDepth = -1)
        {
                // Cannot settle sand if the source is blocked
                if(rockMap.Contains(new Position(sandX, sandY)))
                {
                        return false;
                }

                bool floorExists = floorDepth > 0;
                int maxDepth;

                if (floorExists)
                {
                        maxDepth = floorDepth;
                }
                else
                {
                        maxDepth = rockMap.Select(pos => pos.Y).Max() + 1;
                }

                while(sandY + 1 < maxDepth)
                {
                        // Move down if there is a free space
                        if (!rockMap.Contains(new Position(sandX, sandY + 1)))
                        {
                                sandY++;
                                continue;
                        }

                        // Sand is blocked directly below; try to move diagonally down

                        // First, consider moving left and down
                        if (!rockMap.Contains(new Position(sandX - 1, sandY + 1))) // Left-Down is free and sand keeps falling
                        {
                                sandY++;
                                sandX--;
                                continue;
                        }

                        // Next, consider moving right and down
                        if (!rockMap.Contains(new Position(sandX + 1, sandY + 1))) // Right-Down is free and sand keeps falling
                        {
                                sandX++;
                                sandY++;
                                continue;
                        }

                        // Sand is blocked, and settles in its current position
                        rockMap.Add(new Position(sandX, sandY));
                        return true;
                }

                if (floorExists)
                {
                        // Sand settles on the floor
                        rockMap.Add(new Position(sandX, floorDepth-1));
                        return true;
                }
                else
                {
                        // Sand fell past the lowest rock and never settles
                        return false;
                }
        }

        static void Main(string[] args)
        {
                const bool IS_FOR_PART_1 = false;


                // Parse input into rock structures
                using StreamReader input = new("input.txt");
                List<RockStrucure> rockStrucures = new();
                
                while (!input.EndOfStream)
                {
                        rockStrucures.Add(RockStrucure.Parse(input.ReadLine()));
                }

                // Construct a map of all rock positions from the structures
                HashSet<Position> rockMap = ConstructMap(rockStrucures);

                int floorDepth;
                if (IS_FOR_PART_1)
                {
                        floorDepth = -1;
                }
                else
                {
                        floorDepth = rockMap.Select(pos => pos.Y).Max() + 2;
                }

                // Simulate the falling sand
                int numSandSettled;
                for (numSandSettled = 0; TrySettleSand(500,0, rockMap, floorDepth); numSandSettled++) ;

                Console.WriteLine(numSandSettled);
        }
}