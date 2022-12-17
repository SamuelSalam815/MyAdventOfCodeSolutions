namespace Day16;

internal class ValveNetworkBuilder
{
        List<Valve> valves;
        Dictionary<string, Valve> labelToValveMap;
        Dictionary<string, List<string>> connections;
        bool hasBeenBuilt;

        public ValveNetworkBuilder()
        {
                valves = new List<Valve>();
                labelToValveMap = new Dictionary<string, Valve>();
                connections = new Dictionary<string, List<string>>();
                hasBeenBuilt = false;
        }

        public void ParseValveDefinition(string inputLine)
        {
                if (hasBeenBuilt)
                {
                        throw new Exception("Cannot parse more input once instance has been built");
                }

                string[] stringParts = inputLine.Split(';', 2);
                string valveDefinition = stringParts[0];
                string connectionDefinition = stringParts[1];

                // Parse valve
                stringParts = valveDefinition.Split(' ', 3);
                string valveLabel = stringParts[1];
                string valveFlowRateText = stringParts[2].Split('=', 2)[1];

                Valve valve = new(valveLabel, uint.Parse(valveFlowRateText));

                valves.Add(valve);
                labelToValveMap.Add(valveLabel, valve);

                // Parse connections
                string connectionListText = connectionDefinition[" tunnels lead to valves".Length..];
                connections.Add(valveLabel, connectionListText.Split(", ").Select(label=>label.Trim()).ToList());
        }

        public ValveNetwork Build()
        {
                hasBeenBuilt = true;

                Dictionary<Valve, HashSet<Valve>> valveConnections = new();

                foreach(Valve valve in valves)
                {
                        valveConnections.Add(valve, new());
                }

                foreach( (string valveLabel, List<string> connectedValves) in connections)
                {
                        Valve valve = labelToValveMap[valveLabel];
                        foreach(string connectedValveLabel in connectedValves)
                        {
                                valveConnections[valve].Add(labelToValveMap[connectedValveLabel]);
                        }
                }

                return new ValveNetwork(valves, valveConnections);
        }


}
