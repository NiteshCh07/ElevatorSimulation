using Elevator.Domain;

namespace Elevator.Infrastructure;
public sealed class SimpleConsoleDispatcher : IRequestDispatcher
{
    public void Dispatch(ElevatorRequest request)
    {
        // lightweight: just log; scheduling already placed target stops on elevator model.
        Console.WriteLine($"[Dispatcher] Assigned request: {request}");
    }
}
