namespace Day17;

internal class Day17Solution
{
        public static void Main()
        {
                const long NUM_ROCKS_TO_DROP = 2022;
                // Parse input
                Repeater<long> jetPushRepeater;
                using(StreamReader reader = new("input.txt"))
                {
                        var jetDirectionSequence = reader.ReadLine()
                        .ToArray()
                        .Select(symbol => symbol switch
                        {
                                '<' => -1L,
                                '>' => +1L,
                                _ => throw new Exception("Unrecognized direction")
                        });

                        jetPushRepeater = new Repeater<long>(jetDirectionSequence);
                }
                
                // Generates predetermined sequence of rocks
                Repeater<Rock> rockRepeater = RockRepeaterFactory.GetRepeater();
                
                // Run simulation for the appropiate number of rocks
                Simulator simulator = new(rockRepeater, jetPushRepeater);
                for(long numRocksDropped = 0; numRocksDropped < NUM_ROCKS_TO_DROP; numRocksDropped++)
                {
                        simulator.SettleNextRock();
                }

                Console.WriteLine(simulator.TowerHeight);
        }
}