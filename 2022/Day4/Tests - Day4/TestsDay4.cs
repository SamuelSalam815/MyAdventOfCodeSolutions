using Day4;
namespace TestsDay4;

[TestClass]
public class TestsDay4
{
    [TestMethod]
    [DataRow("2-4,6-8",new int[] {2,4,6,8})]
    [DataRow("2-3,4-5",new int[] {2,3,4,5})]
    [DataRow("5-7,7-9",new int[] {5,7,7,9})]
    [DataRow("3-78,2-99",new int[] {3,78,2,99})]
    public void ParseAssignmentPairs_ReturnsCorrectInput(string input, int[] expectedOutput)
    {
        // Arrange
        // Act
        int[] result = Day4Solution.ParseAssignmentPairs(input);
        // Assert

        CollectionAssert.AreEqual(expectedOutput, result);
    }
}