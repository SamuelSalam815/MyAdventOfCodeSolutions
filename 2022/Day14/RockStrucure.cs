using static Day14.Day14Solution;

namespace Day14
{
        internal class RockStrucure
        {
                // List of the endpoints of the straight lines of rock that make up the structure
                readonly public List<Position> rockPathVertices;

                public RockStrucure(List<Position> rockPathVertices)
                {
                        this.rockPathVertices = new List<Position>(rockPathVertices);
                }

                public List<Position> GetAllRockPositions()
                {
                        List<Position> allRocks = new();
                        Position currentRock = rockPathVertices[0];
                        int xStride;
                        int yStride;
                        foreach(Position lineEndpoint in rockPathVertices)
                        {
                                // Determine the direction to create this line of rocks
                                xStride = 0;
                                yStride = 0;
                                if(currentRock.X != lineEndpoint.X)
                                {
                                        xStride = currentRock.X < lineEndpoint.X ? 1 : -1;
                                }
                                else
                                {
                                        yStride = currentRock.Y < lineEndpoint.Y ? 1 : -1;
                                }

                                // Add rocks until reaching the endpoint
                                while(currentRock != lineEndpoint)
                                {
                                        allRocks.Add(currentRock);
                                        currentRock = new Position(currentRock.X + xStride, currentRock.Y + yStride);
                                }
                        }
                        allRocks.Add(currentRock);
                        return allRocks;
                }

                public static RockStrucure Parse(string inputLine)
                {
                        List<Position> rockPathVertices = new();
                        string[] endPointsAsCoordinates = inputLine.Split(" -> ");
                        foreach (string coordinate in endPointsAsCoordinates)
                        {
                                string[] positions = coordinate.Split(',');
                                int x = int.Parse(positions[0]);
                                int y = int.Parse(positions[1]);
                                rockPathVertices.Add(new Position(x, y));
                        }

                        return new RockStrucure(rockPathVertices);
                }
        }
}
