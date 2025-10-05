using Elevator.Application.UseCases;

namespace Elevator.Infrastructure;
public sealed class ConsoleDisplay
{
    private readonly GetStatusUseCase _statusUseCase;

    public ConsoleDisplay(GetStatusUseCase statusUseCase) => _statusUseCase = statusUseCase;

    public void WriteSnapshot()
    {
        var statuses = _statusUseCase.Execute();
        Console.Clear();
        Console.WriteLine("===== Elevator System Snapshot =====");
        foreach (var s in statuses)
        {
            Console.WriteLine($"Car {s.Id}: floor {s.CurrentFloor} | dir {s.Direction} | pending {s.PendingStops}");
        }
        Console.WriteLine("====================================");
        Console.WriteLine("Press Ctrl+C to quit.");
    }
}
