using Microsoft.Win32.SafeHandles;
using System.Linq;

namespace Day16;

public class Day16Solution
{
        const bool SHOULD_PRINT_TO_CONSOLE = false;

        static void PrintLine(string s)
        {
                if (SHOULD_PRINT_TO_CONSOLE)
                {
                        Console.WriteLine(s);
                }
        }
        public static void Main()
        {

                const int OVERALL_TIME_LIMIT = 26;
                const int NUM_AGENTS = 2;
                const string INPUT_FILE = "input.txt";

                // Parse input
                Dictionary<string, Valve> valveMap = new();
                using (StreamReader inputReader = new(INPUT_FILE))
                {
                        string? line;
                        while ((line = inputReader.ReadLine()) is not null)
                        {
                                Valve currentValve = Valve.Parse(line);
                                valveMap.Add(currentValve.Label, currentValve);
                        }
                }

                // Map pairs of valve labels to the time taken to move between them
                Dictionary<(string, string), uint> travelTime = new();
                foreach ((string label, _) in valveMap)
                {
                        // Precompute shortest distances with breadth first traversal
                        BreadthFirstTraversal(label, valveMap, travelTime);
                }

                // Set up starting state
                uint greatestPressureReleased = 0;
                
                List<string> agentStartingValveLabels = new();
                for(int agentIndex = 0; agentIndex < NUM_AGENTS; agentIndex++)
                {
                        agentStartingValveLabels.Add("AA"); // "AA" is the starting valve
                }

                List<string> initiallyClosedValves = new();
                foreach((_, Valve valve) in valveMap)
                {
                        // Exclude valves with 0 flow rate
                        if (valve.FlowRate > 0)
                        {
                                initiallyClosedValves.Add(valve.Label);
                        }
                }

                // Perform a backtracking traversal of decision tree

                Stack<CheckPoint> checkPoints = new(CreateCheckPointsForEveryPossibleDecision(
                        0, 
                        OVERALL_TIME_LIMIT, 
                        initiallyClosedValves, 
                        agentStartingValveLabels,
                        new List<ValveTask>(),
                        travelTime
                ));

                while(checkPoints.Count > 0)
                {
                        checkPoints.Pop().Deconstruct(
                                out uint totalPressureReleased,
                                out uint minutesRemaining,
                                out List<string> closedValves,
                                out List<string> locationsOfIdlingAgents,
                                out List<ValveTask> activeValveTasks
                        );

                        // Attempt to cull this branch of decision tree, using an upper bound on flow rate
                        List<uint> flowRates = new();

                        flowRates.AddRange(closedValves.Select(label => valveMap[label].FlowRate));
                        flowRates.AddRange(activeValveTasks.Select(task => valveMap[task.TargetValveLabel].FlowRate));
                        flowRates.Sort(new Comparison<uint>((a,b) => b.CompareTo(a))); // Highest pressure first

                        ulong maxPressureBound = totalPressureReleased;
                        
                        for(int flowIndex = 0; flowIndex < flowRates.Count; flowIndex++)
                        {
                                if(flowIndex + 1 >= minutesRemaining)
                                {
                                        break;
                                }
                                maxPressureBound += (ulong)(minutesRemaining - flowIndex - 1) * flowRates[flowIndex]; 
                        }

                        if (maxPressureBound < greatestPressureReleased)
                        {
                                continue;
                        }

                        // Passed cull check

                        void printUpdate()
                        {
                                PrintLine($"Minutes remaining: {minutesRemaining}");
                                PrintLine($"Released pressure: {totalPressureReleased}");
                                PrintLine($"Assignable Valves: {string.Join(", ",closedValves)}");
                                PrintLine("Idle Agent Locations: [" + string.Join(", ",locationsOfIdlingAgents) + "]" );
                        }

                        PrintLine("\n\n---\n\n");
                        printUpdate();
                        PrintLine("Active Tasks:\n" + string.Join("\n", activeValveTasks));

                        // Skip minutes until the next task is completed
                        uint minutesToSkip = activeValveTasks.Select(t => t.MinutesToCompleteTask).Min();

                        List<ValveTask> updatedTasks = activeValveTasks.Select(t => t.MakeProgress(minutesToSkip)).ToList();
                        minutesRemaining -= minutesToSkip;

                        // Now determine which agents have been freed from their tasks
                        List<ValveTask> persistantTasks = new();
                        foreach(ValveTask task in updatedTasks)
                        {
                                if(task.MinutesToCompleteTask == 0)
                                {
                                        locationsOfIdlingAgents.Add(task.TargetValveLabel);
                                        totalPressureReleased += valveMap[task.TargetValveLabel].FlowRate * minutesRemaining;
                                }
                                else
                                {
                                        persistantTasks.Add(task);
                                }
                        }

                        PrintLine($"\nSkipped {minutesToSkip} minutes ahead ...\n");
                        printUpdate();
                        PrintLine("Persistant Tasks:\n" + string.Join("\n", persistantTasks));

                        List<CheckPoint> checkpointsFromPossibleDecisions = CreateCheckPointsForEveryPossibleDecision
                        (
                                totalPressureReleased,
                                minutesRemaining,
                                closedValves,
                                locationsOfIdlingAgents,
                                persistantTasks,
                                travelTime
                        );

                        foreach(CheckPoint checkPoint in checkpointsFromPossibleDecisions)
                        {
                                checkPoints.Push(checkPoint);
                        }

                        if( checkpointsFromPossibleDecisions.Count == 0 )
                        {
                                PrintLine("No more decisions left to make");

                                // When there are no more decisions to make, complete remaining tasks
                                foreach(ValveTask finalTask in persistantTasks)
                                {
                                        if(finalTask.MinutesToCompleteTask < minutesRemaining)
                                        {
                                                totalPressureReleased += valveMap[finalTask.TargetValveLabel].FlowRate * (minutesRemaining - finalTask.MinutesToCompleteTask);
                                        }
                                }

                                greatestPressureReleased = Math.Max(greatestPressureReleased, totalPressureReleased);

                                PrintLine($"Total Pressure Released : {totalPressureReleased}");
                                
                                PrintLine("\n == Back tracking == \n");
                        }
                }

                Console.WriteLine(greatestPressureReleased);
        }

        static void BreadthFirstTraversal(string startLabel, Dictionary<string, Valve> valveMap, in Dictionary<(string, string), uint> travelTime)
        {
                HashSet<string> labelsSeen = new();
                Queue<string> labelQueue = new();

                labelQueue.Enqueue(startLabel);
                labelsSeen.Add(startLabel);

                uint minutesTaken = 0;
                int numValvesAtThisInstantOfTime = 1;
                while (labelQueue.Count > 0)
                {
                        Valve currentValve = valveMap[labelQueue.Dequeue()];

                        travelTime.Add((startLabel, currentValve.Label), minutesTaken);

                        foreach (string label in currentValve.AdjacentValveLabels)
                        {
                                if (!labelsSeen.Contains(label))
                                {
                                        labelQueue.Enqueue(label);
                                        labelsSeen.Add(label);
                                }
                        }

                        numValvesAtThisInstantOfTime--;
                        if (numValvesAtThisInstantOfTime == 0)
                        {
                                numValvesAtThisInstantOfTime = labelQueue.Count;
                                minutesTaken++;
                        }
                }
        }

        static List<CheckPoint> CreateCheckPointsForEveryPossibleDecision
        (
                uint totalPressureReleased,
                uint minutesRemaining,
                List<string> labelsOfClosedValves,
                List<string> valveLabelsOfIdlingAgents, // The labels of valves where free agents are positioned
                List<ValveTask> persistantValveTasks,
                Dictionary<(string, string), uint> travelTime
        )
        {
                List<CheckPoint> checkPoints = new();
                List<List<ValveTask>> possibleDecisions = MakeAllPossibleDecisions(minutesRemaining, labelsOfClosedValves, valveLabelsOfIdlingAgents, travelTime);

                foreach(List<ValveTask> decision in possibleDecisions)
                {
                        List<string> updatedValveLabelsOfIdlingAgents = new(valveLabelsOfIdlingAgents);
                        List<string> updatedLabelsOfClosedValves = new(labelsOfClosedValves);

                        foreach(ValveTask pendingTask in decision)
                        {
                                updatedValveLabelsOfIdlingAgents.Remove(pendingTask.StartingValveLabel);
                                updatedLabelsOfClosedValves.Remove(pendingTask.TargetValveLabel);
                        }

                        decision.AddRange(persistantValveTasks);
                        checkPoints.Add
                        (
                                new CheckPoint
                                (
                                        TotalPressureReleased: totalPressureReleased,
                                        MinutesRemaining: minutesRemaining,
                                        ClosedValves: updatedLabelsOfClosedValves,
                                        LocationsOfIdlingAgents: updatedValveLabelsOfIdlingAgents,
                                        ActiveValveTasks: decision
                                )
                        );
                }

                return checkPoints;
        }

        static List<List<ValveTask>> MakeAllPossibleDecisions
        (
                uint minutesRemaining,
                List<string> labelsOfClosedValves,
                List<string> valveLabelsOfIdlingAgents, // The labels of valves where free agents are positioned
                Dictionary<(string, string), uint> travelTime
        )
        {
                int numFreeAgents = valveLabelsOfIdlingAgents.Count;

                if (numFreeAgents > 2 || numFreeAgents == 0)
                {
                        throw new Exception("Number of free agents not supported");
                }
                if (numFreeAgents == 1)
                {
                        return MakeAllPossibleDecisionsForOneAgent(minutesRemaining, labelsOfClosedValves, valveLabelsOfIdlingAgents[0], travelTime);
                }

                // Number of agents == 2
                List<List<ValveTask>> possibleDecisions = new();

                string valveLabelOfAgent1 = valveLabelsOfIdlingAgents[0];
                string valveLabelOfAgent2 = valveLabelsOfIdlingAgents[1];

                // When there are not enough closed valves to assign to all agents, must allow one agent to idle
                if(labelsOfClosedValves.Count < numFreeAgents)
                {
                        // Consider only using the first agent
                        possibleDecisions.AddRange
                                (MakeAllPossibleDecisionsForOneAgent(minutesRemaining, labelsOfClosedValves, valveLabelOfAgent1, travelTime));

                        // Consider only using the second agent
                        possibleDecisions.AddRange
                                (MakeAllPossibleDecisionsForOneAgent(minutesRemaining, labelsOfClosedValves, valveLabelOfAgent2, travelTime));

                        return possibleDecisions;
                }

                // Consider assigning both agents to different valves - create tasks for opening a pair of valves
                if (labelsOfClosedValves.Count < numFreeAgents)
                {
                        return possibleDecisions; // Cannot assign both agents when there are not enough targets left
                }

                foreach (string labelOfTargetValve1 in labelsOfClosedValves)
                {
                        foreach (string labelOfTargetValve2 in labelsOfClosedValves) // iterate over all possible pairs of valves
                        {
                                if (labelOfTargetValve1.Equals(labelOfTargetValve2))
                                {
                                        continue; // May not assign both agents to the same valve
                                }

                                // To open a valve an agent must travel to the valve and then take an extra minute to open the valve
                                uint minutesForAgent1ToCompleteTask = travelTime[(valveLabelOfAgent1, labelOfTargetValve1)] + 1;
                                uint minutesForAgent2ToCompleteTask = travelTime[(valveLabelOfAgent2, labelOfTargetValve2)] + 1;

                                // For this decision, both agents must be able to complete their tasks on time
                                // If they cannot, do not consider this decision
                                if (minutesForAgent1ToCompleteTask >= minutesRemaining || minutesForAgent2ToCompleteTask >= minutesRemaining)
                                {
                                        continue;
                                }

                                possibleDecisions.Add(new List<ValveTask> {
                                        new ValveTask(valveLabelOfAgent1, labelOfTargetValve1, minutesForAgent1ToCompleteTask),
                                        new ValveTask(valveLabelOfAgent2, labelOfTargetValve2, minutesForAgent2ToCompleteTask),
                                });
                        }
                }

                return possibleDecisions;

        }

        static List<List<ValveTask>> MakeAllPossibleDecisionsForOneAgent
        (
                uint minutesRemaining,
                List<string> labelsOfClosedValves,
                string labelOfStartingValve,
                Dictionary<(string, string), uint> travelTime
        )
        {
                List<List<ValveTask>> possibleDecisions = new();

                foreach (string labelOfTargetValve in labelsOfClosedValves)
                {
                        // It takes an extra minute to open a valve once the agent is at the location
                        uint minutesToCompleteTask = travelTime[(labelOfStartingValve, labelOfTargetValve)] + 1;

                        // If the valve cannot be left open for 1 minute or more, then no pressure can be released
                        // Therefore this decision is not worth considering
                        if (minutesToCompleteTask >= minutesRemaining)
                        {
                                continue;
                        }

                        possibleDecisions.Add(new List<ValveTask> { new ValveTask(labelOfStartingValve, labelOfTargetValve, minutesToCompleteTask) });
                }

                return possibleDecisions;
        }

}