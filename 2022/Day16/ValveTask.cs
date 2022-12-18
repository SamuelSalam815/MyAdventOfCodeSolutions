namespace Day16;

public record struct ValveTask(string StartingValveLabel, string TargetValveLabel, uint MinutesToCompleteTask)
{
        public ValveTask MakeProgress( uint minutesOfProgressMade)
        {
                return new ValveTask(StartingValveLabel, TargetValveLabel, MinutesToCompleteTask - minutesOfProgressMade);
        }
}
