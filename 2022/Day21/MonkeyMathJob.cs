using System.ComponentModel;

namespace Day21;

internal class MonkeyMathJob
{
    public readonly string MonkeyName;
    public readonly string ArgumentName1;
    public readonly string ArgumentName2;
    public readonly OperationType OperationType;
    public readonly MonkeyJobCoordinator Coordinator;
    
    private long? argument1;
    private long? argument2;

    public MonkeyMathJob(string MonkeyName, string ArgumentName1, string ArgumentName2, OperationType OperationType, MonkeyJobCoordinator Coordinator)
    {
        this.MonkeyName = MonkeyName;
        this.ArgumentName1 = ArgumentName1;
        this.ArgumentName2 = ArgumentName2;
        this.OperationType = OperationType;
        this.Coordinator = Coordinator;
        
        this.argument1 = null;
        this.argument2 = null;

        Coordinator.RegisterMonkeyMathJob(this);
    }

    public void SetArgument(string ArgumentName, long value)
    {
        if(ArgumentName1.Equals(ArgumentName))
        {
            argument1 = value;
        }
        
        if(ArgumentName2.Equals(ArgumentName))
        {
            argument2 = value;
        }

        if(argument1 is not null && argument2 is not null)
        {
            Coordinator.TriggerMonkeyShout(MonkeyName, GetNumberToShout());
        }
    }

    private long GetNumberToShout()
    {
        if(argument1 is null || argument2 is null)
        {
            throw new InvalidOperationException("Cannot get the number to shout without dependencies shouting first");
        }
        long value1 = (long)argument1;
        long value2 = (long)argument2;

        return OperationType switch
        {
            OperationType.Add => value1 + value2,
            OperationType.Subtract => value1 - value2,
            OperationType.Multiply => value1 * value2,
            OperationType.Divide => value1 / value2,
            _ => throw new InvalidDataException("Unsupported monkey operation")
        };
    }
}
