using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace Day17;

public class Simulator
{
        // Fields for running the simulation
        private HashSet<Position> occupiedPositions;
        private const int CHAMBER_WIDTH = 7;
        private Repeater<Rock> rockRepeater;
        private Repeater<long> jetPushRepeater; 
        public long TowerHeight { get; private set; }
        private SnapShot? cyclicallyEquivalentSimulatorState;

        private long NumRocksSettled;

        // Fields for simplifying the simulation
        private List<(int, int)> repeaterIndexHistory;

        private record SnapShot
        (
                HashSet<Position> PositionsRelativeToCullHeight,
                long MinAllowedHeight, // The height value used to decide whether a position gets culled
                long TowerHeight,
                long NumRocksSettled
        );

        private List<SnapShot> historyOfSimulatorStateAfterCulling;

        private Position topMostPosition;       // One of the rocks at the TowerHeight

        public Simulator(Repeater<Rock> rockRepeater, Repeater<long> jetPushRepeater)
        {
                this.rockRepeater = rockRepeater;
                this.jetPushRepeater = jetPushRepeater;
                occupiedPositions = new();
                repeaterIndexHistory = new() {};
                topMostPosition = new Position(0,0);
                historyOfSimulatorStateAfterCulling = new();
                NumRocksSettled = 0;
                cyclicallyEquivalentSimulatorState = null;
        }

        public void SettleNextRock()
        {
                Rock currentRock = SpawnRockInPosition();

                //PrintChamberState(currentRock);
                cyclicallyEquivalentSimulatorState = null;
                while (true)
                {
                        long deltaX = jetPushRepeater.NextValue();

                        // Try pushing the rock
                        if(currentRock.MinY - 1 > TowerHeight)
                        {
                                if( 0 < currentRock.MinX + deltaX && currentRock.MaxX + deltaX <= CHAMBER_WIDTH)
                                {
                                        currentRock.Translate(deltaX, 0);
                                }
                        }
                        else
                        {
                                currentRock.Translate(deltaX, 0);
                                if(!IsRockInValidPosition(currentRock))
                                {
                                        currentRock.Translate(-deltaX, 0);
                                }
                        }


                        // Try dropping the rock
                        if(currentRock.MinY - 1 > TowerHeight)
                        {
                                currentRock.Translate(0, -1);
                        }
                        else
                        {
                                currentRock.Translate(0, -1);
                                if (!IsRockInValidPosition(currentRock)) // When dropping causes an invalid position, we have settled the rock
                                {
                                        currentRock.Translate(0, 1);

                                        if(TowerHeight < currentRock.MaxY)
                                        {
                                                TowerHeight = currentRock.MaxY;
                                        }

                                        foreach(Position position in currentRock)
                                        {
                                                occupiedPositions.Add(position);
                                                if(position.Y > topMostPosition.Y)
                                                {
                                                        topMostPosition = position;
                                                }
                                        }

                                        NumRocksSettled++;

                                        TrySimplifyingSimulation();
                                        return;
                                }
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

        // Try to simplify the simulation by culling positions rocks can no longer settle above
        // Also looks for cycles in simulation state

        // Reset the positions vertical values relative to the lowest position not culled
        // Preserve the height of newly settling rocks by using the newRockHeightModifier
        // Look for a cycle in how rocks are settling by comparing current rock position snapshot with the last
        private void TrySimplifyingSimulation()
        {
                // Perform a breadth first search on starting from the highest position
                // If this search reaches both walls of the chamber, we may perform a culling

                Stack<Position> positionsToVisit = new();
                HashSet<Position> seenSet = new();

                bool hasLeftWallBeenVisited = false;
                bool hasRightWallBeenVisited = false;
                long minHeightAllowed = long.MaxValue;

                positionsToVisit.Push(topMostPosition);
                seenSet.Add(topMostPosition);

                while(positionsToVisit.Count > 0)
                {
                        Position nextPosition = positionsToVisit.Pop();

                        if(nextPosition.Y < minHeightAllowed)
                        {
                                minHeightAllowed = nextPosition.Y;
                        }

                        if(nextPosition.X == 1)
                        {
                                hasLeftWallBeenVisited = true;
                        }

                        if(nextPosition.X == CHAMBER_WIDTH)
                        {
                                hasRightWallBeenVisited = true;
                        }

                        if(hasLeftWallBeenVisited && hasRightWallBeenVisited)
                        {
                                break;
                        }

                        Position[] positionsToConsider = new Position[] // Search diagonally and cardinally adjacent positions
                        {
                                new(nextPosition.X - 1, nextPosition.Y + 1),
                                new(nextPosition.X, nextPosition.Y + 1),
                                new(nextPosition.X + 1, nextPosition.Y + 1),

                                new(nextPosition.X + 1, nextPosition.Y),
                                new(nextPosition.X - 1, nextPosition.Y),

                                new(nextPosition.X - 1, nextPosition.Y - 1),
                                new(nextPosition.X, nextPosition.Y - 1),
                                new(nextPosition.X + 1, nextPosition.Y - 1),
                        };

                        foreach(Position candidate in positionsToConsider)
                        {
                                if (seenSet.Contains(candidate))
                                {
                                        continue;
                                }
                                if (!occupiedPositions.Contains(candidate))
                                {
                                        continue;
                                }
                                seenSet.Add(candidate);
                                positionsToVisit.Push(candidate);
                        }
                }

                if(!hasLeftWallBeenVisited || !hasRightWallBeenVisited)
                {
                        return;
                }

                // If we visit both walls of the chamber, the positions below the ones we have visited are definitely unreachable
                if(
                historyOfSimulatorStateAfterCulling.Count > 0 
                &&
                minHeightAllowed <= historyOfSimulatorStateAfterCulling.Last().MinAllowedHeight // If we do not increase the minimum allowed height, no culling will occur
                ) 
                {
                        return;
                }

                HashSet<Position> simplifiedOccupiedPositions = new();
                foreach(Position position in occupiedPositions)
                {
                        // Positions below the lowest we traveresed must be unreachable, so remove them
                        if(position.Y >= minHeightAllowed)
                        {
                                simplifiedOccupiedPositions.Add(position);
                        }
                }

                occupiedPositions = simplifiedOccupiedPositions;
                HashSet<Position> heightCorrectedPositions = occupiedPositions.Select(p => new Position(p.X, p.Y - minHeightAllowed)).ToHashSet();

                foreach(SnapShot historicState in historyOfSimulatorStateAfterCulling)
                { 
                        if (heightCorrectedPositions.SetEquals(historicState.PositionsRelativeToCullHeight))
                        {
                                // If the positions tracked after this culling matches those after the previous culling, correcting for height, then we have a cycle in simulation state;
                                cyclicallyEquivalentSimulatorState = historicState;
                                historyOfSimulatorStateAfterCulling.Clear();
                                break;
                        }
                }

                historyOfSimulatorStateAfterCulling.Add(new(heightCorrectedPositions, minHeightAllowed, TowerHeight, NumRocksSettled));
        }

        public bool TryToExploitRockSettlingCycle(long maxNumberOfRocksToSettle, out long actualNumberOfRocksSettled)
        {
                actualNumberOfRocksSettled = 0;
                if(cyclicallyEquivalentSimulatorState is null)
                {
                        return false;
                }

                long rocksSettledPerCycle = NumRocksSettled - cyclicallyEquivalentSimulatorState.NumRocksSettled;

                if(maxNumberOfRocksToSettle < rocksSettledPerCycle)
                {
                        return false;
                }

                long towerGrowthPerCycle = TowerHeight - cyclicallyEquivalentSimulatorState.TowerHeight;
                long totalTowerGrowth = (maxNumberOfRocksToSettle / rocksSettledPerCycle) * towerGrowthPerCycle;

                TowerHeight += totalTowerGrowth;

                HashSet<Position> positionsAtNewElevation = new();
                foreach(Position position in cyclicallyEquivalentSimulatorState.PositionsRelativeToCullHeight)
                {
                        long newHeight = position.Y;
                        newHeight += cyclicallyEquivalentSimulatorState.MinAllowedHeight; // get the true elevation at that point in time
                        newHeight += (TowerHeight - cyclicallyEquivalentSimulatorState.TowerHeight); // add the tower growth from that point in time to now
                        positionsAtNewElevation.Add(new(position.X, newHeight));
                }

                occupiedPositions = positionsAtNewElevation;
                
                actualNumberOfRocksSettled = maxNumberOfRocksToSettle - (maxNumberOfRocksToSettle % rocksSettledPerCycle);
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
