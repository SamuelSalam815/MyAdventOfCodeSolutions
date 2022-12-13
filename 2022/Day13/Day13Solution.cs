namespace Day13;

internal class Day13Solution
{
        static void Main(string[] args)
        {
                StreamReader input = new("input.txt");
                int sumOfIndicesOfInOrderPairs = 0;
                int packetIndex = 1;

                List<PuzzlePacketData> allPackets = new();
                while (!input.EndOfStream)
                {
                        string? packet1Unparsed = input.ReadLine();
                        if (packet1Unparsed.Equals(string.Empty))
                        {
                                continue;
                        }
                        string? packet2Unparsed = input.ReadLine();

                        PuzzlePacketData packet1 = PuzzlePacketData.Parse(packet1Unparsed);
                        PuzzlePacketData packet2 = PuzzlePacketData.Parse(packet2Unparsed);

                        allPackets.Add(packet1);
                        allPackets.Add(packet2);

                        if(packet1.CompareTo(packet2) < 0) // If in order
                        {
                                sumOfIndicesOfInOrderPairs += packetIndex;
                        }


                        packetIndex++;
                }

                Console.WriteLine(sumOfIndicesOfInOrderPairs);

                PuzzlePacketData divider1 = PuzzlePacketData.Parse("[[2]]");
                PuzzlePacketData divider2 = PuzzlePacketData.Parse("[[6]]");
                allPackets.Add(divider1);
                allPackets.Add(divider2);
                allPackets.Sort((a, b) => a.CompareTo(b));
                int decoderKey = (1+allPackets.IndexOf(divider1)) * (1+allPackets.IndexOf(divider2));

                Console.WriteLine(decoderKey);
        }
}