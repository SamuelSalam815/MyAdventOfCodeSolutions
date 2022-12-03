using RockPaperScissors;
namespace Tests;

[TestClass]
public class TestsDay2
{
    [TestMethod]
    [DataRow(Selection.Rock,Selection.Rock,Result.Draw)]
    [DataRow(Selection.Rock,Selection.Paper,Result.Loss)]
    [DataRow(Selection.Rock,Selection.Scissors,Result.Win)]
    [DataRow(Selection.Paper,Selection.Rock,Result.Win)]
    [DataRow(Selection.Paper,Selection.Paper,Result.Draw)]
    [DataRow(Selection.Paper,Selection.Scissors,Result.Loss)]
    [DataRow(Selection.Scissors,Selection.Rock,Result.Loss)]
    [DataRow(Selection.Scissors,Selection.Paper,Result.Win)]
    [DataRow(Selection.Scissors,Selection.Scissors,Result.Draw)]
    public void GetRoundResult_ReturnsCorrectValues(Selection mainSelection, Selection opposingSelection, Result expectedResult)
    {
        // Arrange
        // Act
        Result actualResult = Day2.GetRoundResult(mainSelection,opposingSelection);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult);
    }

    [TestMethod]
    [DataRow(Selection.Paper,Result.Win,8)]
    [DataRow(Selection.Rock,Result.Loss,1)]
    [DataRow(Selection.Scissors,Result.Draw,6)]
    public void GetRoundScore_ReturnsCorrectValues(Selection mainSelection, Result result, int expectedScore)
    {
        // Arrange
        // Act
        int actualScore = Day2.GetRoundScore(mainSelection,result);
        
        // Assert
        Assert.AreEqual(expectedScore, actualScore);
    }

    [TestMethod]
    [DataRow("A Y",8)]
    [DataRow("B X",1)]
    [DataRow("C Z",6)]
    public void Part1ParseStrategyLine_ReturnsCorrectValues(string line, int expectedScore)
    {
        // Arrange
        // Act
        int actualScore = Day2.ParseStrategyLine(line,isForPart1: true);

        // Assert
        Assert.AreEqual(expectedScore, actualScore);
    }

    [TestMethod]
    [DataRow("A Y", 4)]
    [DataRow("B X", 1)]
    [DataRow("C Z", 7)]
    public void Part2ParseStrategyLine_ReturnsCorrectValues(string line, int expectedScore)
    {
        // Arrange
        // Act
        int actualScore = Day2.ParseStrategyLine(line, isForPart1: false);

        // Assert
        Assert.AreEqual(expectedScore, actualScore);
    }

}