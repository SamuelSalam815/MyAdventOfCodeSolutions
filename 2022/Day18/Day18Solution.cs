namespace Day18;

internal class Day18Solution
{
        private record struct Position(float X, float Y, float Z);

        static Position ParseCube(string cubeText)
        {
                string[] coordsAsText = cubeText.Split(',');

                return new(
                        float.Parse(coordsAsText[0]),
                        float.Parse(coordsAsText[1]),
                        float.Parse(coordsAsText[2])
                );
        }

        private static List<Position> GetFaces(Position cube)
        {
                return new()
                        {
                                new(cube.X, cube.Y, cube.Z + 0.5f),
                                new(cube.X, cube.Y, cube.Z - 0.5f),
                                new(cube.X, cube.Y + 0.5f, cube.Z),
                                new(cube.X, cube.Y - 0.5f, cube.Z),
                                new(cube.X + 0.5f, cube.Y, cube.Z),
                                new(cube.X - 0.5f, cube.Y, cube.Z),
                        };
        }

        static void Main(string[] args)
        {
                HashSet<Position> surfacePositions = new();
                HashSet<Position> exposedPositions = new();
                using StreamReader input = new("example.txt");
                string? line;

                // Define a bounding box that contains all droplets
                // Initially the bounding box is infinitely large
                float minX = float.MaxValue;
                float minY = float.MaxValue;
                float minZ = float.MaxValue;

                float maxX = float.MinValue;
                float maxY = float.MinValue;
                float maxZ = float.MinValue;

                // This function resizes our bounding box to be
                // As small as possible with the given faces
                void UpdateBoundaryCoordinates(Position position)
                {
                        minX = minX < position.X ? minX : position.X;
                        minY = minY < position.Y ? minY : position.Y;
                        minZ = minZ < position.Z ? minZ : position.Z;

                        maxX = maxX > position.X ? maxX : position.X;
                        maxY = maxY > position.Y ? maxY : position.Y;
                        maxZ = maxZ > position.Z ? maxZ : position.Z;
                }

                // Iterate over the input file 
                while((line = input.ReadLine()) is not null)
                {
                        Position cubePosition = ParseCube(line);
                        foreach(Position cubeFace in GetFaces(cubePosition))
                        {
                                UpdateBoundaryCoordinates(cubePosition);
                                if (!surfacePositions.Add(cubeFace))
                                {
                                        surfacePositions.Remove(cubeFace);
                                }
                        }
                }
                // Widen the bounding box, to allow a layer of exposed surfaces

                minX--;
                maxX++;

                minY--;
                maxY++;

                minZ--;
                maxZ++;

                // Now we know if we choose a cube on the edge of the bounding box 
                // it is guaranteed to be in free space and not trapped in a lava bubble

                // Performing a search on adjacent points from this space will
                // reveal all of the faces that are exposed to air

                Position startingPosition = new(minX, minY, minZ);

                HashSet<Position> seenPositions = new();
                Stack<Position> positionsToVisit = new();

                seenPositions.Add(startingPosition);
                positionsToVisit.Push(startingPosition);

                while(positionsToVisit.Count > 0)
                {
                        // TODO : perform a search on all free cubes
                        // if the free cube has a face that is a surface, mark that surface as exposed
                        // only expand on faces that are not surfaces

                        bool TryExpandInDirection(float xStep, float yStep, float zStep)
                        {
                                return false;
                        }
                }

                Console.WriteLine($" Total surface area : {surfacePositions.Count}");
                Console.WriteLine($" Total EXPOSED surface area : {exposedPositions.Count}");
        }
        private static bool IsInRange(float valueToTest, float max, float min)
        {
                return min <= valueToTest && valueToTest <= max;
        }
}