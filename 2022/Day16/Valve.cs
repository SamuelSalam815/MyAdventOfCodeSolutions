using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16;

internal class Valve
{
        public readonly string Label;
        public readonly int FlowRate;
        public readonly int Index;
        public HashSet<Valve> NextValves;

        public Valve(string label, int flowRate, int index)
        {
                Label = label;
                FlowRate = flowRate;
                Index = index;
                NextValves = new();
        }

        static public Valve Parse(string input, int valveIndex)
        {
                string label = input.Substring("Valve ".Length, 2);
                string currentFlowRate = string.Empty;

                for (int charIndex = "Valve AA has flow rate=".Length; charIndex < input.Length; charIndex++)
                {
                        if ('0' <= input[charIndex] && input[charIndex] <= '9')
                        {
                                currentFlowRate += input[charIndex];
                        }
                        else
                        {
                                break;
                        }
                }

                Valve valve = new(label, int.Parse(currentFlowRate), valveIndex);

                return valve;
        }

        public override string ToString()
        {
                return $"{Label}; {FlowRate}; Next=[{string.Join(", ", NextValves.Select(v=>v.Label))}]";
        }
}
