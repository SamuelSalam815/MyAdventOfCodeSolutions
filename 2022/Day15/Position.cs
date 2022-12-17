using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15;
public record struct Position(int X, int Y)
{
    public int ManhattanDistance(Position other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }
}
