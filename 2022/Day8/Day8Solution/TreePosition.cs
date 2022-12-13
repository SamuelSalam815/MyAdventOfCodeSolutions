using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day8Solution
{
    internal readonly record struct TreePosition(int RowIndex, int ColumnIndex);

    internal static class TreePositionExtentions
    {

        /// <summary>
        /// Returns whether a tree in a given position is visible to an observer in a given direction relative to the tree.
        /// An observer north of a tree has a field of view pointing south.
        /// It is assumed the observer lies in a cardinally aligned line from the tree, is outside the bounds of the height map, and is looking at the tree.
        /// </summary>
        /// <param name="position">the position of the tree we are determining is visible or not</param>
        /// <param name="observerRelativeToTree">The direction, relative to the tree, an observer is trying to observe the tree from</param>
        /// <param name="heightMap">defines the heights of trees at each valid position</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool IsVisibleFrom(this TreePosition position, Direction observerRelativeToTree, char[][] heightMap, out int directionalScenicScore)
        {
            // Assume height map is ordered North-Most row first, West-Most column first, with rows being the first index and columns the second index.

            // Given the information about the observer, infer one of the coordinates. The other is specified by Index
            // Expect iteration over the rows when considering North/South directions
            // Expect iteration over the columns when considering East/West directions
            char GetHeightOfTreeAt(int index)
            {
                return observerRelativeToTree switch
                {
                    Direction.North or Direction.South => heightMap[index][position.ColumnIndex],
                    Direction.East or Direction.West => heightMap[position.RowIndex][index],
                    _ => throw new Exception("Unknown Direction"),
                };
            }

            // Stride in a direction towards the observer, relative to the given tree position
            // Striding north of the tree requires the row index to decrease
            // Striding west of the tree requires the column index to decrease
            int stride = observerRelativeToTree switch
            {
                Direction.North or Direction.West => -1,
                Direction.South or Direction.East => 1,
                _ => throw new Exception("Unknown Direction"),
            };

            int startingIterationValue = observerRelativeToTree switch
            {
                Direction.North or Direction.South => position.RowIndex,
                Direction.East or Direction.West => position.ColumnIndex,
                _ => throw new Exception("Unknown Direction"),
            };

            // Defining the first invalid index we expect to encounter when starting at the given position
            int terminationValue = observerRelativeToTree switch
            {
                Direction.South => heightMap[0].Length,
                Direction.East => heightMap.Length,
                Direction.North or Direction.West => -1,
                _ => throw new Exception("Unknown Direction"),
            };

            char positionTreeHeight = heightMap[position.RowIndex][position.ColumnIndex];
            directionalScenicScore = 0;
            for(int currentIndex = startingIterationValue + stride; currentIndex != terminationValue; currentIndex += stride)
            {
                directionalScenicScore++;
                if(positionTreeHeight <= GetHeightOfTreeAt(currentIndex))
                {
                    return false;
                }
            }

            return true;

        }
    }
}
