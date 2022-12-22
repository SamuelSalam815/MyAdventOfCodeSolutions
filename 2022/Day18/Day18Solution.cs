namespace Day18;

internal static class Day18Solution
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

        static void Main()
        {
                HashSet<Position> lavaDropletSurfacePositions = new();
                HashSet<Position> exposedPositions = new();
                using StreamReader input = new("input.txt");
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
                // As small as possible for the given inputs
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
                                if (!lavaDropletSurfacePositions.Add(cubeFace))
                                {
                                        lavaDropletSurfacePositions.Remove(cubeFace);
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
                        Position currentCubePosition = positionsToVisit.Pop();

                        foreach(Position cubeFace in GetFaces(currentCubePosition))
                        {
                                // If the free cube has a face that is a surface, mark that surface as exposed
                                if(lavaDropletSurfacePositions.Contains(cubeFace))
                                {
                                        exposedPositions.Add(cubeFace);
                                        // Only expand on faces that are not surfaces
                                        continue;
                                }

                                // Calculating the adjacent cube position, depending on the face position

                                float xStride = cubeFace.X - currentCubePosition.X;
                                float yStride = cubeFace.Y - currentCubePosition.Y;
                                float zStride = cubeFace.Z - currentCubePosition.Z;

                                Position adjacentFreeCube = new (cubeFace.X + xStride, cubeFace.Y + yStride, cubeFace.Z + zStride);

                                // Do not expand to cubes out of bounds
                                if( minX > adjacentFreeCube.X || maxX < adjacentFreeCube.X)
                                {
                                        continue;
                                }
                                if( minY > adjacentFreeCube.Y || maxY < adjacentFreeCube.Y)
                                {
                                        continue;
                                }
                                if( minZ > adjacentFreeCube.Z || maxZ < adjacentFreeCube.Z)
                                {
                                        continue;
                                }

                                // Do not expand to cubes we have already planned to visit
                                if(seenPositions.Contains(adjacentFreeCube))
                                {
                                        continue;
                                }

                                seenPositions.Add(adjacentFreeCube);
                                positionsToVisit.Push(adjacentFreeCube);
                        }
                }

                Console.WriteLine($"Total surface area : {lavaDropletSurfacePositions.Count}");
                Console.WriteLine($"Total EXPOSED surface area : {exposedPositions.Count}");
        }
}