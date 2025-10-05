using System.Collections.Concurrent;

namespace Elevator.Domain;
public sealed class ElevatorRun
{
    public int Id { get; }
    public int CurrentFloor { get; private set; }
    public ElevatorDirection Direction { get; private set; } = ElevatorDirection.Idle;

    // internal queue of stops the elevator must serve (simple)
    private readonly ConcurrentQueue<int> _stops = new();

    public bool IsIdle => Direction == ElevatorDirection.Idle && _stops.IsEmpty;

    public ElevatorRun(int id, int initialFloor = 1)
    {
        Id = id;
        CurrentFloor = initialFloor;
    }

    public void EnqueueStop(int floor)
    {
        _stops.Enqueue(floor);
        UpdateDirection();
    }

    public bool TryDequeueNextStop(out int nextFloor)
    {
        if (_stops.TryDequeue(out nextFloor))
        {
            UpdateDirection();
            return true;
        }
        nextFloor = CurrentFloor;
        Direction = ElevatorDirection.Idle;
        return false;
    }

    // Used by scheduler to peek how many stops or distance
    public int PendingStopsCount => _stops.Count;

    public void MoveOneFloorTowards(int targetFloor)
    {
        if (targetFloor > CurrentFloor) { CurrentFloor++; Direction = ElevatorDirection.Up; }
        else if (targetFloor < CurrentFloor) { CurrentFloor--; Direction = ElevatorDirection.Down; }
        else { /* already there */ }
    }

    private void UpdateDirection()
    {
        // Determine basic direction based on next queued stop, if any.
        if (_stops.TryPeek(out var next))
            Direction = next > CurrentFloor ? ElevatorDirection.Up : (next < CurrentFloor ? ElevatorDirection.Down : Direction);
        else
            Direction = ElevatorDirection.Idle;
    }

    public override string ToString() => $"[Car {Id}] Floor {CurrentFloor} ({Direction}) pending:{PendingStopsCount}";
}
