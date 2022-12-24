namespace Day19.ProductionSimulation;

public record struct Simulation(
        ResourceGrowthTracker OreGrowthTracker,
        ResourceGrowthTracker ClayGrowthTracker,
        ResourceGrowthTracker ObsidianGrowthTracker,
        ResourceGrowthTracker GeodeGrowthTracker,
        Blueprint Blueprint)
{
        public Simulation GrowAllResources() => new(
                OreGrowthTracker.Grow(),
                ClayGrowthTracker.Grow(),
                ObsidianGrowthTracker.Grow(),
                GeodeGrowthTracker.Grow(),
                Blueprint);
}