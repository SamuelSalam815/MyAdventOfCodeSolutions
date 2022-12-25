namespace Day19.Resources;

public record struct ResourceStore(
        ResourceIdentifier ResourceType,
        int UnitsStored,
        int UnitsFoundPerCollection
)
{
        public bool TryConsume(int numUnitsToConsume, out ResourceStore updatedStore)
        {
                updatedStore = new ResourceStore();
                if (UnitsStored < numUnitsToConsume)
                {
                        return false;
                }
                updatedStore = new(ResourceType, UnitsStored - numUnitsToConsume, UnitsFoundPerCollection);
                return true;
        }

        public ResourceStore IncrementProductionLevel() => new(ResourceType, UnitsStored, UnitsFoundPerCollection + 1);

        public ResourceStore CollectMore() => new(ResourceType, UnitsStored + UnitsFoundPerCollection, UnitsFoundPerCollection);
}