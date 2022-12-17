namespace Day16;

public class ValveNetwork
{
        public Dictionary<Valve, HashSet<Valve>> ValveConnections;
        public List<Valve> AllValves;
        public Dictionary<(Valve, Valve), uint> ShortestDistances;

        public ValveNetwork(List<Valve> allValves, Dictionary<Valve, HashSet<Valve>> valveConnections)
        {
                this.ValveConnections = valveConnections;
                this.AllValves = allValves;
                ShortestDistances = new();
                InitializeShortestDistances();
        }
        
        private void InitializeShortestDistances()
        {
                foreach(Valve valve in AllValves)
                {
                        InitializeShortestDistancesFrom(valve);
                }
        }

        // Find shortest distances by breadth first
        private void InitializeShortestDistancesFrom(Valve startingValve)
        {
                HashSet<Valve> seenValves = new();
                Queue<Valve> valvesToTraverse = new();

                seenValves.Add(startingValve);
                valvesToTraverse.Enqueue(startingValve);

                uint currentDistance = 0;
                int valvesAtThisDistance = 1;

                while(valvesToTraverse.Count > 0)
                {
                        Valve currentValve = valvesToTraverse.Dequeue();
                        
                        if (!ShortestDistances.TryGetValue( (startingValve,currentValve), out uint _ ))
                        {
                                ShortestDistances.Add((startingValve, currentValve), currentDistance);
                        }

                        foreach(Valve adjacentValve in ValveConnections[currentValve])
                        {
                                if (!seenValves.Contains(adjacentValve))
                                {
                                        seenValves.Add(adjacentValve);
                                        valvesToTraverse.Enqueue(adjacentValve);
                                }
                        }

                        valvesAtThisDistance--;
                        if(valvesAtThisDistance == 0)
                        {
                                currentDistance++;
                                valvesAtThisDistance = valvesToTraverse.Count;
                        }
                }
        }

}
