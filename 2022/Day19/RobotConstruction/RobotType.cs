using Day19.Resources;

namespace Day19.RobotConstruction;

public enum RobotType : byte
{
    Ore = 1,
    Clay = 2,
    Obsidian = 3,
    Geode = 4
}

public static class RobotTypeExtensions
{
        public static ResourceType ResourceGathered(this RobotType robot ) => robot switch
        {
                RobotType.Ore => ResourceType.Ore,
                RobotType.Clay => ResourceType.Clay,
                RobotType.Obsidian => ResourceType.Obsidian,
                RobotType.Geode => ResourceType.Geode,
                _ => throw new ArgumentException("Unsupported robot type"),
        };
}