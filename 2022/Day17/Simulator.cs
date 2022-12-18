using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace Day17;

public class Simulator
{
        private HashSet<Position> occupiedPositions;
        private const int CHAMBER_WIDTH = 7;
        private Repeater<Rock> rockRepeater;
        private Repeater<long> jetPushRepeater; 
        public long TowerHeight { get; private set; }

        public Simulator(Repeater<Rock> rockRepeater, Repeater<long> jetPushRepeater)
        {
                this.rockRepeater = rockRepeater;
                this.jetPushRepeater = jetPushRepeater;
                occupiedPositions = new();
        }

        public void SettleNextRock()
        {
                Rock currentRock = SpawnRockInPosition();

                //PrintChamberState(currentRock);

                while (true)
                {
                        long xStride = jetPushRepeater.NextValue();

                        // Try pushing the rock
                        Rock adjustedRock = TranslateRockPosition(currentRock, xStride, 0);
                        if( IsRockInValidPosition(adjustedRock))
                        {
                                currentRock = adjustedRock;
                        }

                        adjustedRock = TranslateRockPosition(currentRock, 0, -1);
                        if( IsRockInValidPosition(adjustedRock))
                        {
                                currentRock = adjustedRock;
                        }
                        else
                        {
                                foreach(Position position in currentRock)
                                {
                                        if(position.Y > TowerHeight)
                                        {
                                                TowerHeight = position.Y;
                                        }

                                        occupiedPositions.Add(position);
                                }
                                return;
                        }
                }
        }

        private Rock SpawnRockInPosition()
        {
                Rock currentRockTemplate = rockRepeater.NextValue();

                long leftMostCoordinate = long.MaxValue;
                long bottomMostCoordinate = long.MaxValue;

                foreach (Position position in currentRockTemplate)
                {
                        leftMostCoordinate = Math.Min(leftMostCoordinate, position.X);
                        bottomMostCoordinate = Math.Min(bottomMostCoordinate, position.Y);
                }

                long xTranslation = (3) - leftMostCoordinate;
                long yTranslation = (4 + TowerHeight) - bottomMostCoordinate;

                Rock rockToSpawn = new();

                foreach (Position position in currentRockTemplate)
                {
                        rockToSpawn.Add(new Position(position.X + xTranslation, position.Y + yTranslation));
                }

                return rockToSpawn;
        }

        private static Rock TranslateRockPosition(Rock originalRock, long xTranslation, long yTranslation)
        {
                Rock adjustedRock = new();
                foreach (Position position in originalRock)
                {
                        adjustedRock.Add(new Position(position.X + xTranslation, position.Y + yTranslation));
                }
                return adjustedRock;
        }

        private bool IsRockInValidPosition(Rock rock)
        {
                foreach (Position position in rock)
                {
                        if (!IsPositionInBounds(position) || occupiedPositions.Contains(position))
                        {
                                return false;
                        }
                }
                return true;
        }

        private static bool IsPositionInBounds(Position position)
        {
                if(position.X < 1 || position.X > CHAMBER_WIDTH)
                {
                        return false;
                }

                if(position.Y < 1)
                {
                        return false;
                }

                return true;
        }

        private void PrintChamberState(Rock fallingRock)
        {
                long highestY = Math.Max(TowerHeight, fallingRock.Select(p => p.Y).Max());

                for(long currentY = highestY; currentY > 0; currentY--)
                {
                        Console.Write('|');
                        for(long x = 1; x <= CHAMBER_WIDTH; x++)
                        {
                                Position currentPosition = new(x, currentY);
                                if (fallingRock.Contains(currentPosition))
                                {
                                        Console.Write('@');
                                }
                                else if (occupiedPositions.Contains(currentPosition))
                                {
                                        Console.Write('#');
                                }
                                else
                                {
                                        Console.Write('.');
                                }
                        }
                        Console.WriteLine('|');
                }

                Console.Write('+');
                for(int i = 0; i < CHAMBER_WIDTH; i++)
                {
                        Console.Write('-');
                }
                Console.WriteLine("+");
        }
}
