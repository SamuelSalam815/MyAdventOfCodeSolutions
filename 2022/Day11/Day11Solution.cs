namespace Day11
{
    internal class Day11Solution
    {

        static void Main(string[] args)
        {
            bool isForPart1 = false;
            
            int numRoundsToSimulate;
            long worryReliefDivisor = -1;
            if (isForPart1)
            {
                numRoundsToSimulate = 20;
            } else // Part 2
            {
                numRoundsToSimulate = 10_000;
            }

            // Parse input
            List<Monkey> monkies = new();
            using StreamReader inputStream = new("input.txt");
            while (!inputStream.EndOfStream)
            {
                monkies.Add(MonkeyParser.Parse(inputStream));
                inputStream.ReadLine(); // Skip blank line
            }

            if (!isForPart1)
            {
                worryReliefDivisor = monkies.Select(m => m.throwTestDivisor).Aggregate(1, (long acc, long x) => acc * x);
            }

            // Simulate item throwing rounds
            for(int roundCount = 1; roundCount <= numRoundsToSimulate; roundCount++)
            {
                for(int monkeyId = 0; monkeyId < monkies.Count; monkeyId++)
                {
                    monkies[monkeyId].TakeTurn(monkies,worryReliefDivisor);
                }

                if(roundCount == 1 || roundCount == 20 || roundCount % 1000 == 0)
                {
                    Console.WriteLine($"== After round {roundCount} ==");
                    for(int monkeyId = 0; monkeyId < monkies.Count; monkeyId++)
                    {
                        Console.WriteLine($"Monkey {monkeyId} inspected items {monkies[monkeyId].GetNumItemInspections()} times.");
                    }
                }
            }

            List<long> inspectionCounts =
                monkies
                .Select(x => x.GetNumItemInspections())
                .ToList();

            inspectionCounts.Sort();

            long monkeyBusinessLevel = inspectionCounts[inspectionCounts.Count - 1] * inspectionCounts[inspectionCounts.Count - 2];
            Console.WriteLine($"Level of monkey business {monkeyBusinessLevel}");
        }
    }
}