using ElevatorSimulation.Models;
using ElevatorSimulation.Utils;

namespace ElevatorSimulation.Simulation
{
    public class RandomRequestGenerator
    {
        private readonly List<Elevator> _elevators;
        private readonly int _numFloors;
        private readonly Random _random = new Random();

        public RandomRequestGenerator(int numElevators, int numFloors)
        {
            _numFloors = numFloors;
            _elevators = new List<Elevator>();

            for (int i = 1; i <= numElevators; i++)
            {
                _elevators.Add(new Elevator(i));
            }
        }

        public async Task RunAsync(int numberOfRequests)
        {
            for (int i = 0; i < numberOfRequests; i++)
            {
                int floor = _random.Next(1, _numFloors + 1);
                Direction dir = floor == _numFloors ? Direction.Down :
                                floor == 1 ? Direction.Up :
                                (_random.Next(0, 2) == 0 ? Direction.Up : Direction.Down);

                Logger.Log($"Request: {dir} at floor {floor}");

                AssignRequest(floor);

                // let all elevators make a step
                var tasks = _elevators.Select(e => e.StepAsync());
                await Task.WhenAll(tasks);
            }
        }

        private void AssignRequest(int floor)
        {
            // naive strategy: choose nearest elevator
            var chosen = _elevators
                .OrderBy(e => Math.Abs(e.CurrentFloor - floor))
                .First();

            chosen.AddRequest(floor);
        }
    }
}
