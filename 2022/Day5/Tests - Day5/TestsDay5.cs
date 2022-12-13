using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace Tests___Day5
{
    [TestClass]
    public class TestsDay5
    {
        [TestMethod]
        [DataRow(
            new string[] {
            "    [D]    ",
            "[N] [C]    ",
            "[Z] [M] [P]",
            " 1   2   3 "
            },
            new char[]{'N','Z'},
            new char[]{'D','C','M'},
            new char[]{'P'}
        )]
        [DataRow(
            new string[] {
            "[D]        ",
            "[N] [C]    ",
            "[Z] [M] [P]",
            " 1   2   3 "
            },
            new char[] { 'D', 'N', 'Z' },
            new char[] { 'C', 'M' },
            new char[] { 'P' }
        )]
        public void ParseStartingStack_ReturnsCorrectResults(string[] input, params char[][] expectedOutput )
        {
            // Arrange
            // Act

            char[][] output = Day5Solution.ParseStartingStack(input);

            // Assert
            Assert.AreEqual(expectedOutput.Length, output.Length);
            for(int i = 0; i < expectedOutput.Length; i++)
            {
                CollectionAssert.AreEqual(expectedOutput[i], output[i]);
            }
        }

        [TestMethod]
        [DataRow("move 1 from 2 to 1", 1,2,1)]
        [DataRow("move 3 from 2 to 3", 3,2,3)]
        [DataRow("move 21 from 2 to 1", 21,2,1)]
        [DataRow("move 1 from 25 to 1", 1,25,1)]
        public void ParseInstructionLine_ReturnsCorrectResults(string input, int qty, int source, int target)
        {
            // Arrange
            Day5Solution.Instruction expectedOutput = new(qty, source, target);
            // Act
            Day5Solution.Instruction output = Day5Solution.ParseInstruction(input);

            // Assert
            Assert.AreEqual(expectedOutput, output);
        }
    }
}