using Elevator.Domain;

namespace Elevator.Infrastructure;
public sealed class InMemoryElevatorRepository : IElevatorRepository
{
    private readonly List<ElevatorRun> _elevators;

    public InMemoryElevatorRepository(int count, int initialFloor = 1)
    {
        _elevators = Enumerable.Range(1, count).Select(i => new ElevatorRun(i, initialFloor)).ToList();
    }

    public IReadOnlyList<ElevatorRun> GetAll() => _elevators.AsReadOnly();

    public ElevatorRun? GetById(int id) => _elevators.FirstOrDefault(e => e.Id == id);
}
