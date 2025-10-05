using Elevator.Domain;

namespace Elevator.Application.Services;

/// <summary>
/// Simple scheduler/worker that moves elevators step-by-step (simulated time).
/// It reads from the in-memory repository to service stops. It uses IClock for testability.
/// </summary>
public sealed class SimpleScheduler
{
    private readonly IElevatorRepository _repo;
    private readonly IClock _clock;
    private readonly int _moveMsPerFloor = 10_000;
    private readonly int _stopMs = 10_000;

    public SimpleScheduler(IElevatorRepository repo, IClock clock)
    {
        _repo = repo;
        _clock = clock;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        // naive loop: step each elevator towards its next stop
        while (!ct.IsCancellationRequested)
        {
            var elevators = _repo.GetAll();

            foreach (var elevator in elevators)
            {
                if (elevator.TryDequeueNextStop(out var targetFloor))
                {
                    // move until reach targetFloor
                    while (elevator.CurrentFloor != targetFloor && !ct.IsCancellationRequested)
                    {
                        elevator.MoveOneFloorTowards(targetFloor);
                        // infrastructure (console) will read repository for status
                        await _clock.DelayAsync(_moveMsPerFloor, ct);
                    }

                    // arrived, simulate stop time
                    await _clock.DelayAsync(_stopMs, ct);
                }
            }

            // small tick to avoid busy spin
            await _clock.DelayAsync(200, ct);
        }
    }
}
