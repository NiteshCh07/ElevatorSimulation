using Elevator.Domain;
public interface IRequestDispatcher
{
    /// <summary>Dispatch request to an elevator (assign)</summary>
    void Dispatch(ElevatorRequest request);
}
