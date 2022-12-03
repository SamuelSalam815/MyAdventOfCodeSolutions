namespace Tests
{
    [TestClass]
    public class TestsDay3
    {
        [TestMethod]
        [DataRow("vJrwpWtwJgWrhcsFMMfFFhFp", 'p')]
        [DataRow("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", 'L')]
        [DataRow("PmmdzqPrVvPwwTWBwg", 'P')]
        [DataRow("wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", 'v')]
        [DataRow("ttgJtRGJQctTZtZT", 't')]
        [DataRow("CrZsJsPPZsGzwwsLwLmpwMDw", 's')]
        public void FindRucksackOutlier_ReturnsCorrectCompartments(string rucksack, char expectedOutlier)
        {
            // Arrange
            // Act
            char actualOutlier = Day3.FindRucksackOutlier(rucksack);
            // Assert

            Assert.AreEqual(expectedOutlier, actualOutlier);
        }
    }
}