namespace Day15;

internal class Program
{
    static void Main()
    {
        Part1(2_000_000, "input.txt");
        Part2(20, "example.txt");
        Part2(4_000_000, "input.txt");
    }

    static void Part1(int rowOfInterest, string inputFilePath)
    {
        // Parse input line by line
        StreamReader input = new(inputFilePath);
        List<ExclusionZone> zones = new();
        HashSet<Position> beacons = new();

        string? line;
        while ((line = input.ReadLine()) is not null)
        {
            ExclusionZone zone = ExclusionZone.Parse(line);
            zones.Add(zone);
            beacons.Add(zone.Beacon);
        }

        // Now calculate the number of excluded positions at the given row
        HashSet<Position> excludedPositions = new();
        foreach (ExclusionZone zone in zones)
        {
            zone.GetAllExcludedPositionsOnRow(rowOfInterest, excludedPositions, beacons);
        }

        Console.WriteLine(excludedPositions.Count);
    }

    static void Part2(int searchBoxLength, string inputFilePath)
    {

        // TODO: Optimize by using boxes for fast discard of candidate position
        StreamReader input = new(inputFilePath);
        List<ExclusionZone> zones = new();

        string? line;
        while ((line = input.ReadLine()) is not null)
        {
            ExclusionZone zone = ExclusionZone.Parse(line);
            zones.Add(zone);
        }

        for(int yCandidatePosition = 0; yCandidatePosition <= searchBoxLength; yCandidatePosition++)
        {
            for(int xCandidatePosition = 0; xCandidatePosition <= searchBoxLength; xCandidatePosition++)
            {
                Position candidatePosition = new(xCandidatePosition, yCandidatePosition);
                foreach(ExclusionZone zone in zones)
                {
                    if (zone.Detects(candidatePosition))
                    {
                        // Skip the rest of the positions that will be detected on this row
                        int verticalDistance = Math.Abs(yCandidatePosition - zone.Sensor.Y);
                        int zoneWidthOnThisRow = zone.ManhattanRadius - verticalDistance;
                        xCandidatePosition = zone.Sensor.X + zoneWidthOnThisRow;

                        goto DetectedBySensor;
                    }
                }
                // Found the distress signal
                // Position is not at a seen beacon
                // And position is not in any exclusion zone
                ulong tuningFrequency = (ulong)candidatePosition.X * 4_000_000UL + (ulong)candidatePosition.Y;
                Console.WriteLine(tuningFrequency);
                return;
            DetectedBySensor:;
            }
        }
    }
}