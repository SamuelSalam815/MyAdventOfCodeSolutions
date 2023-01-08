namespace Day19.Resources;

public record struct ResourceStore(
        ResourceType ResourceType,
        int UnitsStored,
        int UnitsProducedPerMinute
)
{
        public ResourceStore AdvanceTime(int minutesPassed)
        {
                return new(ResourceType, UnitsStored + (minutesPassed * UnitsProducedPerMinute), UnitsProducedPerMinute);
        }

        public ResourceStore IncrementProduction()
        {
                return new(ResourceType, UnitsStored, UnitsProducedPerMinute + 1);
        }

        public ResourceStore Consume(int quantityConsumed)
        {
                return new(ResourceType, UnitsStored - quantityConsumed, UnitsProducedPerMinute);
        }
};