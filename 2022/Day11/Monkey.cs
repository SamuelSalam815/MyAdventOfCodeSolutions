using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    internal class Monkey
    {
        private readonly List<long> items;
        private long numItemInspections;

        private readonly long? inspectionArgument1;
        private readonly long? inspectionArgument2;

        public enum InspectionType
        {
            Add,
            Multiply
        }

        private readonly InspectionType inspectionType;

        public readonly long throwTestDivisor;
        private readonly int firstThrowTarget;
        private readonly int secondThrowTarget;

        public Monkey(List<long> startingItems, long? inspectionArgument1, long? inspectionArgument2, InspectionType inspectionType, long throwTestDivisor, int firstThrowTarget, int secondThrowTarget)
        {
            items = new List<long>(startingItems);
            this.inspectionArgument1 = inspectionArgument1;
            this.inspectionArgument2 = inspectionArgument2;
            this.inspectionType = inspectionType;
            this.throwTestDivisor = throwTestDivisor;
            this.firstThrowTarget = firstThrowTarget;
            this.secondThrowTarget = secondThrowTarget;

            numItemInspections = 0;
        }

        public void AddItem(long itemWorryLevel)
        {
            items.Add(itemWorryLevel);
        }

        private long Inspect(long worryLevel)
        {
            long actualArgument1;

            if(inspectionArgument1 is null)
            {
                actualArgument1 = worryLevel;
            } else
            {
                actualArgument1 = (long)inspectionArgument1;
            }
            
            long actualArgument2;

            if(inspectionArgument2 is null)
            {
                actualArgument2 = worryLevel;
            } else
            {
                actualArgument2 = (long)inspectionArgument2;
            }

            numItemInspections++;

            return inspectionType switch
            {
                InspectionType.Add => actualArgument1 + actualArgument2,
                InspectionType.Multiply => actualArgument1 * actualArgument2,
                _ => throw new Exception("Unknown inspection type")
            };
        }

        private void ThrowItem(long worryLevel, List<Monkey> allMonkies)
        {
            if(worryLevel % throwTestDivisor == 0)
            {
                allMonkies[firstThrowTarget].AddItem(worryLevel);
            }
            else
            {
                allMonkies[secondThrowTarget].AddItem(worryLevel);
            }
        }

        public void TakeTurn(List<Monkey> allMonkies, long worryReliefDivisor = -1)
        {
            foreach(long originalWorryLevel in items)
            {
                long worryLevel = Inspect(originalWorryLevel);

                // Worry Relief
                if (worryReliefDivisor == -1)
                {
                    worryLevel /= 3;
                } else
                {
                    worryLevel %= worryReliefDivisor;
                }

                // Throw item
                ThrowItem(worryLevel, allMonkies);
            }

            items.Clear();
        }

        public long GetNumItemInspections() => numItemInspections;

        public override string ToString()
        {
            string inspectionArgument1String = inspectionArgument1 is null ? "old" : inspectionArgument1.ToString();
            string inspectionArgument2String = inspectionArgument2 is null ? "old" : inspectionArgument2.ToString();

            return 
                $"{{ {String.Join(", ", items)} }} "
                +
                $" {{ new = {inspectionArgument1String} {(inspectionType == InspectionType.Add ? '+' : '*')} {inspectionArgument2String} }} "
                +
                $" {{ divisible by {throwTestDivisor} ?  {firstThrowTarget} : {secondThrowTarget} }}";
        }
    }
}
