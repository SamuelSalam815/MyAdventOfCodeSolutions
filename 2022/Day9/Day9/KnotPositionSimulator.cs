using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day9
{
    internal class KnotPositionSimulator
    {
        readonly private HashSet<KnotPosition> positionsLastKnotHasVisited;

        private List<KnotPosition> knots;

        // By default, simulate only two knots.
        public KnotPositionSimulator() : this(2) { }

        public KnotPositionSimulator(int numOfKnots)
        {
            knots = new List<KnotPosition>();

            for(int i = 0; i < numOfKnots; i++)
            {
                knots.Add(new(0, 0));
            }
            
            positionsLastKnotHasVisited = new HashSet<KnotPosition>()
            {
                knots[0]
            };
        }

        // Each pair of knots can be though of as a head-tail pair
        // Head and tail are adjacent in the knot list
        // The head knot comes before the tail knot
        // The tail knot follows the movements of the head knot according to particular rules
        // This method returns the new position of the tail knot
        private static KnotPosition GetNextTailPosition(KnotPosition tail, KnotPosition head)
        {
            if (tail.IsAdjacentTo(head)) // No need for tail to move
            {
                return tail;
            }

            List<(int, int)> candidateMoves;

            if (tail.X == head.X || tail.Y == head.Y) // In same row or column
            {
                candidateMoves = new()
                {
                    (1,0),
                    (-1,0),
                    (0,1),
                    (0,-1),
                };
            }
            else
            {
                candidateMoves = new()
                {
                    (1,1),
                    (-1,1),
                    (1,-1),
                    (-1,-1),
                };
            }

            foreach ((int xMove, int yMove) in candidateMoves)
            {
                KnotPosition candidatePosition = new(tail.X + xMove, tail.Y + yMove);
                if (candidatePosition.IsAdjacentTo(head))
                {
                    return candidatePosition;
                }
            }

            throw new Exception($"No possible move can make the tail {tail} adjacent to the head {head}");
        }

        public void ExecuteHeadMovement(string direction, int repetitions)
        {
            (int xMovement, int yMovement) = direction switch
            {
                "R" => (1, 0),
                "L" => (-1, 0),
                "U" => (0, 1),
                "D" => (0, -1),
                _ => throw new Exception("Unknown Direction"),
            };


            for(int i = 0; i < repetitions; i++)
            {
                // Update head position
                knots[0] = new(knots[0].X + xMovement, knots[0].Y + yMovement);

                // Update entire trail
                for(int knotIndex = 1; knotIndex < knots.Count; knotIndex++)
                {
                    knots[knotIndex] = GetNextTailPosition(tail: knots[knotIndex], head: knots[knotIndex - 1]);
                }
                positionsLastKnotHasVisited.Add(knots.Last());
            }
        }

        public int GetNumberOfDistinctTailPositions() => positionsLastKnotHasVisited.Count;
    }
}
