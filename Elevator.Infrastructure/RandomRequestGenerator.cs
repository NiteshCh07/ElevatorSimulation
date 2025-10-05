using Elevator.Domain;

namespace Elevator.Infrastructure;
public sealed class RandomRequestGenerator
{
    private readonly Random _rnd = new();
    public ElevatorRequest Next()
    {
        int src = _rnd.Next(1, 11);
        int dest;

        do {
            dest = _rnd.Next(1, 11); 
        } while (dest == src);

        return new ElevatorRequest(src, dest);
    }
}
