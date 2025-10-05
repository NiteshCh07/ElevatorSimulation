namespace Elevator.Domain;
public interface IElevatorRepository
{
    IReadOnlyList<ElevatorRun> GetAll();
    ElevatorRun? GetById(int id);
}
