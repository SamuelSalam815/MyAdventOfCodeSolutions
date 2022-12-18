using System.Collections.ObjectModel;

namespace Day16;

public record struct Valve(string Label, uint FlowRate, ReadOnlyCollection<string> AdjacentValveLabels)
{
        static public Valve Parse(string line)
        {
                string valveDescription = line.Split(';')[0];
                
                string label = valveDescription.Split(' ', 3)[1];
                
                string flowRateText = valveDescription.Split('=', 2)[1];
                uint flowRate = uint.Parse(flowRateText);

                string adjacencyDescription = line.Split(';')[1];
                string adjacencyListAsText = adjacencyDescription.Split(' ', 6)[5];
                ReadOnlyCollection<string> adjacencyList = adjacencyListAsText.Split(", ").ToList().AsReadOnly();

                return new Valve(label, flowRate, adjacencyList);
        }

        public override string ToString()
        {
                return $"Valve {{{Label}}}, Flow rate {{{FlowRate}}}, Adjacent to [{string.Join(", ", AdjacentValveLabels)}]";
        }
}

