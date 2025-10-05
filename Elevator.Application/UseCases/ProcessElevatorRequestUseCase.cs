using Elevator.Domain;

namespace Elevator.Application.UseCases;

/// <summary>
/// Use-case: accept a request and schedule an elevator (simple scheduler).
/// </summary>
public sealed class ProcessElevatorRequestUseCase
{
    private readonly IElevatorRepository _repo;
    private readonly IRequestDispatcher _dispatcher;

    public ProcessElevatorRequestUseCase(IElevatorRepository repo, IRequestDispatcher dispatcher)
    {
        _repo = repo;
        _dispatcher = dispatcher;
    }

    public void Handle(ElevatorRequest request)
    {
        // Use a simple policy: choose closest elevator by distance to source that is not moving opposite
        // or choose idle elevator closest. This is intentionally naive but deterministic.

        var elevators = _repo.GetAll();

        // Prefer elevator moving in same direction and will pass by source floor; else choose idle closest; else closest by distance.
        ElevatorRun? selected = null;

        // 1) find elevator moving towards source and same direction
        selected = elevators
            .Where(e => !e.IsIdle && e.Direction == request.Direction)
            .Where(e => request.Direction == ElevatorDirection.Up ? e.CurrentFloor <= request.SourceFloor : e.CurrentFloor >= request.SourceFloor)
            .OrderBy(e => Math.Abs(e.CurrentFloor - request.SourceFloor))
            .FirstOrDefault();

        // 2) otherwise pick idle closest
        selected ??= elevators.Where(e => e.IsIdle)
                              .OrderBy(e => Math.Abs(e.CurrentFloor - request.SourceFloor))
                              .FirstOrDefault();

        // 3) otherwise choose globally closest
        selected ??= elevators.OrderBy(e => Math.Abs(e.CurrentFloor - request.SourceFloor)).First();

        // enqueue pickup then destination
        // basic policy: pick up first, then destination
        selected.EnqueueStop(request.SourceFloor);
        selected.EnqueueStop(request.DestinationFloor);

        // notify dispatcher (infrastructure) so it can start processing
        _dispatcher.Dispatch(request);
    }
}
