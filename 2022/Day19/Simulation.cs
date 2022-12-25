using Day19.Resources;

namespace Day19;

public record struct Simulation(
        Dictionary<ResourceIdentifier, ResourceStore> Resources,
        int TimeUnitsPassed)
{
        public Simulation AdvanceTime() => new(
                Resources
                        .Select(resourceEntry => resourceEntry.Value.CollectMore())
                        .ToDictionary(resourceStore => resourceStore.ResourceType),
                TimeUnitsPassed + 1);

        public bool TryBuildRobot(List<Ingredient> requiredMaterials, out Simulation newSimulationState)
        {
                newSimulationState = new();
                Dictionary<ResourceIdentifier, ResourceStore> newSetOfResourceStores = new(Resources);

                // It takes 1 unit of time to build a robot.
                // We require all the resources necessary resources before beginning the build process
                foreach (Ingredient ingredient in requiredMaterials)
                {
                        ResourceStore store = newSetOfResourceStores[ingredient.ResourceType];
                        if (!store.TryConsume(ingredient.RequiredQuantity, out ResourceStore newStore))
                        {
                                // Insufficient resources stored. Cannot consume the required quantity of resources for this robot
                                return false;
                        }

                        // During the time the robot is being built, more resources are collected
                        newSetOfResourceStores[ingredient.ResourceType] = newStore.CollectMore();
                }

                newSimulationState = new(newSetOfResourceStores, TimeUnitsPassed + 1);
                return true;
        }


}