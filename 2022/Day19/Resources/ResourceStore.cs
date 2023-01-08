namespace Day19.Resources;

public record struct ResourceStore(
        ResourceType ResourceType,
        int UnitsStored,
        int UnitsProducedPerMinute
);