namespace Day16;

public record CheckPoint(
        uint TotalPressureReleased,
        uint MinutesRemaining, 
        List<string> ClosedValves,
        List<string> LocationsOfIdlingAgents,
        List<ValveTask> ActiveValveTasks
);
