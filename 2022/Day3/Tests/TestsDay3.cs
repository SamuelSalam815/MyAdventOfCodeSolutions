using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Tests
{
    [TestClass]
    public class TestsDay3
    {
        [TestMethod]
        [DataRow("vJrwpWtwJgWrhcsFMMfFFhFp", "vJrwpWtwJgWr", "hcsFMMfFFhFp")]
        [DataRow("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", "jqHRNqRjqzjGDLGL", "rsFMfFZSrLrFZsSL")]
        [DataRow("PmmdzqPrVvPwwTWBwg", "PmmdzqPrV", "vPwwTWBwg")]
        public void SplitRucksack_ReturnsCorrectCompartments(string rucksack, string expectedFirstCompartment, string expectedSecondCompartment)
        {
            // Arrange
            // Act
            IEnumerable<string> actualCompartments = Program.SplitRuckSack(rucksack);
            // Assert

            Assert.AreEqual(expectedFirstCompartment, actualCompartments.First(), false);
            Assert.AreEqual(expectedFirstCompartment, actualCompartments.Last(), false);
        }
    }
}