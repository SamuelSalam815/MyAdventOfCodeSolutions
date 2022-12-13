namespace Day4;

public class Day4Solution
{
    public static int[] ParseAssignmentPairs(string line)
    {
        string[] assignments = line.Split(',');
        string[] firstAssignmentBounds = assignments[0].Split('-');
        string[] secondAssignmentBounds = assignments[1].Split('-');

        int[] result = new int[4];
        _ = int.TryParse(firstAssignmentBounds[0], out result[0]);
        _ = int.TryParse(firstAssignmentBounds[1], out result[1]);
        _ = int.TryParse(secondAssignmentBounds[0], out result[2]);
        _ = int.TryParse(secondAssignmentBounds[1], out result[3]);
        return result;
    }

    private static bool RangesAreFullyOverlapping(int[] bounds)
    {
        if (bounds.Length != 4)
        {
            throw new ArgumentException();
        }

        int lowerA = bounds[0];
        int upperA = bounds[1];
        int lowerB = bounds[2];
        int upperB = bounds[3];

        if (lowerA <= lowerB && upperB <= upperA)
        {
            return true;
        }

        if (lowerB <= lowerA && upperA <= upperB)
        {
            return true;
        }

        return false;
    }

    private static bool RangesArePartiallyOverlapping(int[] bounds)
    {
        if (bounds.Length != 4)
        {
            throw new ArgumentException();
        }

        int lowerA = bounds[0];
        int upperA = bounds[1];
        int lowerB = bounds[2];
        int upperB = bounds[3];

        if (upperA < lowerB || lowerA > upperB)
        {
            return false;
        }

        return true;
    }

    private static void Main(string[] args)
    {
        using StreamReader inputStream = new("input.txt");
        string? line;
        int numRangesFullyOverlap = 0;
        while ((line = inputStream.ReadLine()) is not null)
        {
            if (RangesArePartiallyOverlapping(ParseAssignmentPairs(line)))
            {
                numRangesFullyOverlap++;
            }
        }

        Console.WriteLine(numRangesFullyOverlap);
    }
}