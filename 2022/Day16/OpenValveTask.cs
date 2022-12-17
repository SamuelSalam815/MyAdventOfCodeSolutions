namespace Day16;

public record struct OpenValveTask(Valve Target, uint MinutesToComplete)
{
        public OpenValveTask GetTimeAdvancedTask() => new OpenValveTask(Target, MinutesToComplete - 1);
}
