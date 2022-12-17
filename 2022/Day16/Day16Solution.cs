using System.Collections;
using System.Xml.Schema;

namespace Day16;

internal class Day16Solution
{

        static void Main(string[] args)
        {
                // == Parsing puzzle input ==

                StreamReader inputText = new("example.txt");
                List<Valve> allValves = new();
                List<List<string>> indexToNextValveLabelsMap = new();
                string? line;
                while ((line = inputText.ReadLine()) is not null)
                {
                        // Parse valve
                        Valve nextValve = Valve.Parse(line, allValves.Count());
                        allValves.Add(nextValve);

                        // Store the next valves
                        string nextValves = line.Split(';', 2)[1];
                        nextValves = nextValves[(" tunnels lead to valves ".Length - 1)..];
                        indexToNextValveLabelsMap.Add( 
                                nextValves
                                .Split(',')
                                .Select(label => label.Trim())
                                .ToList() 
                        );
                }

                // == Preprocessing ==
                
                // Add references between valves
                for (int valveIndex = 0; valveIndex < allValves.Count; valveIndex++)
                {
                        Valve currentValve = allValves[valveIndex];
                        List<string> labels = indexToNextValveLabelsMap[valveIndex];
                        currentValve.NextValves.AddRange(
                                allValves.Where(v => labels.Contains(v.Label))
                        );
                }

                // Get shortest number of steps from each valve to every other valve
                int[][] shortestDistances = new int[allValves.Count][];
                foreach(Valve valve in allValves)
                {
                        shortestDistances[valve.Index] = new int[allValves.Count];
                        for (int i = 0; i < allValves.Count; i++)
                        {
                                shortestDistances[valve.Index][i] = -1; // Default to unreachable
                        }
                        BreadthFirstTraversal(valve, shortestDistances[valve.Index]);
                }

                // Get the starting valve
                Valve startingValve = allValves.Where(v => v.Label.Equals("AA")).First();

                // == Solving the puzzle ==
                

                BitArray startingOpenValves = new BitArray(allValves.Count, false); // All valves start off closed
                Console.WriteLine( MaximizePressureRelease(30, startingValve, allValves, startingOpenValves, shortestDistances) );
            ;
        }

        // perform a breadth first traversal while recording distances to each encountered node
        static void BreadthFirstTraversal(Valve startingValve, int[] distances)
        {
                HashSet<Valve> seenSet = new();
                seenSet.Add(startingValve);

                Queue<Valve> valvesToTraverse = new();
                valvesToTraverse.Enqueue(startingValve);


                int nodesAtThisLevel = 1;
                int stepsTaken = 0;
                while(valvesToTraverse.Count > 0)
                {
                        Valve currentValve = valvesToTraverse.Dequeue();
                        distances[currentValve.Index] = stepsTaken;

                        foreach(Valve candidateValve in currentValve.NextValves)
                        {
                                if (!seenSet.Contains(candidateValve))
                                {
                                        valvesToTraverse.Enqueue(candidateValve);
                                        seenSet.Add(candidateValve);
                                }
                        }

                        nodesAtThisLevel--;
                        if(nodesAtThisLevel == 0)
                        {
                                stepsTaken++;
                                nodesAtThisLevel = valvesToTraverse.Count;
                        }
                }
        }

        // Find the best moves by trying every move
        // A valid move is travelling to a valve and then opening it
        static int MaximizePressureRelease(int minutesLeft, Valve currentValve, List<Valve> allValves, BitArray openValves, int[][] shortestDistances)
        {
                if (minutesLeft == 0)
                {
                        return 0;
                };

                List<int> possiblePressureReleases = new();

                // Only consider those valves that are ...
                // Reachable
                // Closed
                // Have positive flow

                foreach(Valve candidate in allValves)
                {
                        if (shortestDistances[currentValve.Index][candidate.Index] <= 0)
                        {
                                continue;
                        }

                        if (openValves[candidate.Index])
                        {
                                continue;
                        }

                        if(candidate.FlowRate <= 0)
                        {
                                continue;
                        }

                        // 1 min per unit distance.
                        // 1 min to open the valve.
                        // The rest of the minutes, pressure is released at the flowrate.
                        int minutesOfPressureRelease = minutesLeft - shortestDistances[currentValve.Index][candidate.Index] - 1;
                        int totalPressureReleasedFromCandidate = minutesOfPressureRelease * candidate.FlowRate;
                        
                        BitArray newOpenValves = new(openValves);
                        newOpenValves[candidate.Index] = true;

                        int pressureReleasedFromTheRest = MaximizePressureRelease(minutesOfPressureRelease, candidate, allValves, newOpenValves, shortestDistances);

                        possiblePressureReleases.Add(totalPressureReleasedFromCandidate + pressureReleasedFromTheRest);
                }

                if(possiblePressureReleases.Count == 0)
                {
                        return 0;
                }

                return possiblePressureReleases.Max();
        }
}