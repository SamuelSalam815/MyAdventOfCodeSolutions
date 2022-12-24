namespace Day19.ProductionSimulation;

public record struct ResourceGrowthTracker(
        int NumResourceUnits,
        int NumUnitsMadePerStep
)
{
        public bool TryConsume(int numUnitsToConsume, out ResourceGrowthTracker updatedCounter)
        {
                updatedCounter = new ResourceGrowthTracker();
                if(NumResourceUnits < numUnitsToConsume)
                {
                        return false;
                }
                updatedCounter = new(NumResourceUnits - numUnitsToConsume, NumUnitsMadePerStep);
                return true;
        }

        public ResourceGrowthTracker IncrementProduction() => new(NumResourceUnits, NumUnitsMadePerStep + 1);

        public ResourceGrowthTracker Grow() => new(NumResourceUnits + NumUnitsMadePerStep, NumUnitsMadePerStep);
}