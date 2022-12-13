namespace Day10
{
    internal class Day10Solution
    {
        static void Main(string[] args)
        {
            StreamReader inputFile = new("puzzle input.txt");
            string? line;
            
            CPUSimulation sim = new();

            while((line = inputFile.ReadLine()) is not null)
            {
                string[] commandParts = line.Split(' ', 2);

                switch (commandParts[0])
                {
                    case "noop":
                        sim.ExecuteInstruction(CPUInstructionType.noop);
                        break;
                    case "addx":
                        sim.ExecuteInstruction(CPUInstructionType.addx, int.Parse(commandParts[1]));
                        break;
                    default:
                        throw new Exception($"Unknown instruction {commandParts[0]}");
                }

            }

            // History gives the value of the register AFTER a given number of cycles

            // First item (index 0) is the value after the 0th cycle => this means the initial register value
            // Second item (index 1) is the value after the 1st cycle ...

            // To find the value of the register DURING a cycle, look at its value AFTER the previous cycle
            // Register value cannot change during a cycle
            
            // Example :
            // Finding the register value during the 5th cycle requires the register value after the 4th cycle
            // The value AFTER the 4th cycle is located at index 4
            List<int> registerValueHistory = sim.GetRegisterHistory();
            
            int signalStrengthSum = 0;

            for(int sampleCycle = 20; sampleCycle < registerValueHistory.Count; sampleCycle += 40)
            {
                signalStrengthSum += sampleCycle * registerValueHistory[sampleCycle - 1];
            }

            Console.WriteLine($"Total signal strength : {signalStrengthSum}");


            for(int sampleCycle = 1; sampleCycle <= 240; sampleCycle++)
            {
                int spritePosition = registerValueHistory[sampleCycle - 1];

                int pixelBeingDrawn = (sampleCycle-1) % 40; // Starting pixel position is 0

                if(pixelBeingDrawn - 1 == spritePosition || pixelBeingDrawn == spritePosition || pixelBeingDrawn + 1 == spritePosition)
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }

                if(pixelBeingDrawn == 39)
                {
                    Console.WriteLine();
                }
            }
        }
    }
}