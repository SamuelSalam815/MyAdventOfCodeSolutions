using Day19.Resources;

namespace Day19;

public record struct DecisionNode(
        uint MinutesPassed,
        ResourceStore OreStore,
        ResourceStore ClayStore,
        ResourceStore ObsidianStore,
        ResourceStore GeodeStore)
{
        // The initial state of a 'run'
        // Where a run is the process of collecting a number of geodes with a given blueprint and within a given time
        public DecisionNode() : this(
                MinutesPassed: 0,
                OreStore: new(ResourceType.Ore, 0, 1),
                ClayStore: new(ResourceType.Clay, 0, 0),
                ObsidianStore: new(ResourceType.Obsidian, 0, 0),
                GeodeStore: new(ResourceType.Geode, 0, 0)
        ){ }
}