using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Day9
{
    readonly record struct KnotPosition(int X, int Y);

    internal static class KnotPositionExtentions
    {
        static private int MagnitudeOfDifference(int A, int B)
        {
            int diff = A - B;
                
            if(diff < 0)
            {
                diff = -diff;
            }

            return diff;
        }
        public static bool IsAdjacentTo(this KnotPosition positionA, KnotPosition positionB)
        {

            if (MagnitudeOfDifference(positionA.X,positionB.X) > 1) return false;
            if (MagnitudeOfDifference(positionA.Y,positionB.Y) > 1) return false;

            return true;
        }
    }
}
