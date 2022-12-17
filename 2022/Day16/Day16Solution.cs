#define PART1
namespace Day16;

public class Day16Solution
{
        /// <summary>
        /// Returns all ordered ways of picking <paramref name="quantity"/> separate items from <paramref name="values"/>
        /// </summary>
        /// <typeparam name="T"> The type of the items being picked </typeparam>
        /// <param name="quantity"> how many items to pick </param>
        /// <param name="values"> the collection of available choices </param>
        /// <returns>A list of combinations where each combination is an ordered selection of items from <paramref name="values"/></returns>
        static List<List<T>> GetCombinations<T>(int quantity, ICollection<T> values) where T : IEquatable<T>
        {

                if(quantity > values.Count)
                {
                        throw new Exception($"Cannot choose {quantity} items when only {values.Count} are provided");
                }

                if(quantity == 0)
                {
                        return new List<List<T>> { new List<T>() };
                }

                List<List<T>> result = new();

                foreach(T value in values)
                {
                        List<List<T>> selections = GetCombinations(quantity - 1, values.Where(v => !v.Equals(value)).ToList());

                        foreach(List<T> selection in selections)
                        {
                                List<T> newSelection = new(selection)
                                {
                                        value
                                };

                                result.Add(newSelection);
                        }
                }

                return result;

        }

        public static void Main()
        {
                const int NUM_AGENTS = 2;
                const uint OVERALL_TIME_LIMIT_MINUTES = 26;

                ValveNetwork valveNetwork;
                
                // Parse input
                using (StreamReader input = new("input.txt"))
                {
                        ValveNetworkBuilder networkBuilder = new();
                        string? line;
                        while((line = input.ReadLine()) is not null)
                        {
                                networkBuilder.ParseValveDefinition(line);
                        }

                        valveNetwork = networkBuilder.Build();
                }

                // Run simulations on all possible decisions to determine greatest amount of pressure that can be released

                Stack<Simulation> simulations = new();

                HashSet<Valve> initiallyClosedValves = 
                        valveNetwork
                        .AllValves
                        .Where(v => v.FlowRate > 0)
                        .ToHashSet();
                
                Valve startingValve = 
                        valveNetwork
                        .AllValves
                        .Where(v => v.Label.Equals("AA"))
                        .First();


                // The first decision : which valve to initially open
                // Each active task represents an agent
                foreach(List<Valve> selection in GetCombinations(NUM_AGENTS, initiallyClosedValves) )
                {
                        List<OpenValveTask> tasks = new();
                        HashSet<Valve> targetableValves = new(initiallyClosedValves);

                        foreach(Valve target in selection)
                        {
                                tasks.Add(new OpenValveTask(
                                        Target: target,
                                        MinutesToComplete: valveNetwork.ShortestDistances[(startingValve, target)] + 1
                                ));
                                targetableValves.Remove(target); // Prevent multiple agents from trying to open the same valve at once
                        }

                        simulations.Push(new Simulation(
                                MinutesRemaining: OVERALL_TIME_LIMIT_MINUTES,
                                TotalPressureReleased: 0,
                                TargetableValves: targetableValves,
                                // Targetable valves are not open, are not in the process of being opened and have positive flow
                                RunningTasks: tasks
                        ));
                }

                ulong greatestPressureReleased = 0;

                while(simulations.Count > 0)
                {
                        simulations.Pop().Deconstruct
                        (
                                out uint minutesLeft,
                                out ulong currentPressureReleased,
                                out HashSet<Valve> targetableValves,
                                out List<OpenValveTask> activeTasks
                        );

                        minutesLeft--;
                        List<Valve> startingPositionsForNewTasks = new(); // Each position here corresponds to the position of an idling agent
                        List<OpenValveTask> persistingTasks = new();
                        
                        foreach(OpenValveTask task in activeTasks)
                        {
                                OpenValveTask updatedTesk = task.GetTimeAdvancedTask();
                                if(updatedTesk.MinutesToComplete == 0)
                                {
                                        currentPressureReleased += minutesLeft * task.Target.FlowRate;
                                        startingPositionsForNewTasks.Add(task.Target);
                                } else
                                {
                                        persistingTasks.Add(updatedTesk);
                                }
                        }

                        if(minutesLeft == 0)
                        {
                                greatestPressureReleased = Math.Max(currentPressureReleased, greatestPressureReleased);
                                continue;
                        }

                        int numAgentsAvailable = startingPositionsForNewTasks.Count;

                        if(numAgentsAvailable == 0 || targetableValves.Count == 0) 
                        // Without free agents or more targets, there is only one decision : wait for agents to complete their task
                        {
                                simulations.Push(new Simulation(
                                        MinutesRemaining: minutesLeft,
                                        TotalPressureReleased: currentPressureReleased,
                                        TargetableValves: targetableValves,
                                        RunningTasks: persistingTasks
                                ));
                                continue;
                        }

                        List<List<Valve>> combinations;
                        bool areMoreTargetsThanAgents = numAgentsAvailable <= targetableValves.Count;

                        // There are two set of valves at this point and we wish to enumerate every possible decision
                        // The set of valves that agents can begin tasks from
                        // The set of valves that agents can begin opening
                        // The combinations from the larger set with size equal to the smaller set give all possible task assignment decisions

                        if( areMoreTargetsThanAgents )
                        {
                                combinations = GetCombinations(numAgentsAvailable, targetableValves);
                        }
                        else
                        {
                                combinations = GetCombinations(targetableValves.Count, startingPositionsForNewTasks);
                        }

                        foreach (List<Valve> combination in combinations)
                        {
                                List<OpenValveTask> newTasks = new(persistingTasks);
                                HashSet<Valve> newTargatableValves = new(targetableValves);
                                List<Valve> targetValves = targetableValves.ToList();
                                for(int valveIndex = 0; valveIndex < combination.Count; valveIndex++)
                                {
                                        Valve newStartValve;
                                        Valve newTargetValve;

                                        if(areMoreTargetsThanAgents)
                                        {
                                                newStartValve = startingPositionsForNewTasks[valveIndex];
                                                newTargetValve = combination[valveIndex];
                                        }
                                        else
                                        {
                                                newStartValve = combination[valveIndex];
                                                newTargetValve = targetValves[valveIndex];
                                        }

                                        newTargatableValves.Remove(newTargetValve);

                                        uint timeForCompletion = valveNetwork.ShortestDistances[(newStartValve, newTargetValve)] + 1;
                                        newTasks.Add(new OpenValveTask(newTargetValve,timeForCompletion));
                                }

                                simulations.Push( new Simulation(
                                        MinutesRemaining: minutesLeft,
                                        TotalPressureReleased: currentPressureReleased,
                                        TargetableValves: newTargatableValves,
                                        RunningTasks: newTasks
                                ));
                        }
                }

                Console.WriteLine(greatestPressureReleased);
        }
}