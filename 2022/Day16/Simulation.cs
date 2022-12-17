
namespace Day16;

internal record Simulation(uint MinutesRemaining, ulong TotalPressureReleased, HashSet<Valve> TargetableValves, List<OpenValveTask> RunningTasks);
