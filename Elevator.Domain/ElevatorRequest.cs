namespace Elevator.Domain;
public sealed class ElevatorRequest
{
    public int SourceFloor { get; }
    public int DestinationFloor { get; }
    public ElevatorDirection Direction => DestinationFloor > SourceFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

    public ElevatorRequest(int sourceFloor, int destinationFloor)
    {
        if (sourceFloor < 1 || sourceFloor > 10) throw new ArgumentOutOfRangeException(nameof(sourceFloor));
        if (destinationFloor < 1 || destinationFloor > 10) throw new ArgumentOutOfRangeException(nameof(destinationFloor));
        if (sourceFloor == destinationFloor) throw new ArgumentException("Source and destination must differ.");

        SourceFloor = sourceFloor;
        DestinationFloor = destinationFloor;
    }

    public override string ToString() => $"Request {SourceFloor} -> {DestinationFloor} ({Direction})";
}
