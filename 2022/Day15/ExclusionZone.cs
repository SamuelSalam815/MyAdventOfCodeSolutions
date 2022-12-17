using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15;
public record ExclusionZone(Position Sensor, Position Beacon)
{
    private int _radius = -1;
    public int ManhattanRadius 
    {
        get
        {
            if (_radius != -1)
            {
                return _radius;
            }

            _radius = Sensor.ManhattanDistance(Beacon);
            return _radius;
        }
    }
    public bool Detects(Position position)
    {
        if (Sensor.ManhattanDistance(position) > ManhattanRadius)
        {
            return false;
        }

        return true;
    }

    public void GetAllExcludedPositionsOnRow(int row, in ICollection<Position> output, HashSet<Position>? beacons = null)
    {
        int verticalDistance = Math.Abs(Sensor.Y - row);
        int distanceToBeacon = ManhattanRadius;

        if (verticalDistance > distanceToBeacon)
        {
            return;
        }

        int horizontalLeeway = distanceToBeacon - verticalDistance;

        for(int x = Sensor.X - horizontalLeeway; x <= Sensor.X + horizontalLeeway; x++)
        {
            Position position = new(x, row);
            if (beacons is not null && !beacons.Contains(position))
            {
                output.Add(position);
            }
        }

    }

    static public ExclusionZone Parse(string input)
    {
        int[] coordinates = new int[4];
        int coordinateIndex = 0;
        string currentNumber = string.Empty;
        foreach (char c in input)
        {

            bool characterIsADigit = '0' <= c && c <= '9';
            bool characterIsASign = string.IsNullOrEmpty(currentNumber) && c == '-';

            if (characterIsADigit || characterIsASign)
            {
                currentNumber += c;
            }
            else if (!string.IsNullOrEmpty(currentNumber))
            {
                coordinates[coordinateIndex++] = int.Parse(currentNumber);
                currentNumber = string.Empty;
            }
        }

        // Parse the last coordinate
        coordinates[coordinateIndex] = int.Parse(currentNumber);

        Position sensor = new(coordinates[0], coordinates[1]);
        Position beacon = new(coordinates[2], coordinates[3]);

        return new ExclusionZone(sensor, beacon);

    }
}
