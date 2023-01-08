using Day19.Resources;
using Day19.RobotConstruction;

namespace Day19;

public record struct CollectionState(
        int MinutesPassed,
        ResourceStore OreStore,
        ResourceStore ClayStore,
        ResourceStore ObsidianStore,
        ResourceStore GeodeStore)
{
        // The initial state of a 'run'
        // Where a run is the process of collecting a number of geodes with a given blueprint and within a given time
        public CollectionState() : this(
                MinutesPassed: 0,
                OreStore: new(ResourceType.Ore, 0, 1),
                ClayStore: new(ResourceType.Clay, 0, 0),
                ObsidianStore: new(ResourceType.Obsidian, 0, 0),
                GeodeStore: new(ResourceType.Geode, 0, 0)
        ){ }

        // Higher priority => look at first
        public long Priority =>
        (1_000 * GeodeStore.UnitsProducedPerMinute) +
        (100 * ObsidianStore.UnitsProducedPerMinute) +
        (10 * ClayStore.UnitsProducedPerMinute) +
        (1 * OreStore.UnitsProducedPerMinute);

    public ResourceStore GetStoreForResource(ResourceType resourceType)
    {
        return resourceType switch
        {
            ResourceType.Ore => OreStore,
            ResourceType.Clay => ClayStore,
            ResourceType.Obsidian => ObsidianStore,
            ResourceType.Geode => GeodeStore,
            _ => throw new ArgumentException($"Unsupported resource type {resourceType}")
        };
    }

    public CollectionState AdvanceTime(int additionalMinutes)
    {
        return new(
            MinutesPassed + additionalMinutes,
            OreStore.AdvanceTime(additionalMinutes),
            ClayStore.AdvanceTime(additionalMinutes),
            ObsidianStore.AdvanceTime(additionalMinutes),
            GeodeStore.AdvanceTime(additionalMinutes));
    }

    public CollectionState BuildRobot(RobotType robot, IEnumerable<Ingredient> recipe)
    {
        CollectionState newState = this;
        foreach(Ingredient ingredient in recipe)
        {
                newState = newState.Consume(ingredient);
        }
        newState = newState.AdvanceTime(1); // Robots take 1 minute to build
        return robot switch
        {
                RobotType.Ore => new(newState.MinutesPassed, newState.OreStore.IncrementProduction(), newState.ClayStore, newState.ObsidianStore, newState.GeodeStore),
                RobotType.Clay => new(newState.MinutesPassed, newState.OreStore, newState.ClayStore.IncrementProduction(), newState.ObsidianStore, newState.GeodeStore),
                RobotType.Obsidian => new(newState.MinutesPassed, newState.OreStore, newState.ClayStore, newState.ObsidianStore.IncrementProduction(), newState.GeodeStore),
                RobotType.Geode => new(newState.MinutesPassed, newState.OreStore, newState.ClayStore, newState.ObsidianStore, newState.GeodeStore.IncrementProduction()),
                _ => throw new ArgumentException($"Unsupported robot type {robot}"),
        };
    }

    private CollectionState Consume(Ingredient ingredient)
    {
        return ingredient.ResourceType switch
        {
                ResourceType.Ore => new(MinutesPassed, OreStore.Consume(ingredient.RequiredQuantity), ClayStore, ObsidianStore, GeodeStore),
                ResourceType.Clay => new(MinutesPassed, OreStore, ClayStore.Consume(ingredient.RequiredQuantity), ObsidianStore, GeodeStore),
                ResourceType.Obsidian => new(MinutesPassed, OreStore, ClayStore, ObsidianStore.Consume(ingredient.RequiredQuantity), GeodeStore),
                ResourceType.Geode => new(MinutesPassed, OreStore, ClayStore, ObsidianStore, GeodeStore.Consume(ingredient.RequiredQuantity)),
                _ => throw new ArgumentException($"Unsupported resource type {ingredient.ResourceType}")
        };
    }
}