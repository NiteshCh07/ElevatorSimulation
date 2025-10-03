using ElevatorSimulation.Models;
using ElevatorSimulation.Utils;

namespace ElevatorSimulation
{
    public class Elevator
    {
        public int Id { get; }
        public int CurrentFloor { get; private set; }
        public Direction Direction { get; private set; }

        private readonly Queue<int> _requests = new Queue<int>();

        public Elevator(int id, int startFloor = 1)
        {
            Id = id;
            CurrentFloor = startFloor;
            Direction = Direction.Idle;
        }

        public bool HasRequests => _requests.Count > 0;

        public void AddRequest(int floor)
        {
            if (!_requests.Contains(floor))
            {
                _requests.Enqueue(floor);
                if (Direction == Direction.Idle)
                {
                    Direction = floor > CurrentFloor ? Direction.Up : Direction.Down;
                }
            }
        }

        public async Task StepAsync()
        {
            if (!HasRequests)
            {
                Direction = Direction.Idle;
                return;
            }

            int target = _requests.Peek();

            if (CurrentFloor < target)
            {
                await MoveUpAsync();
            }
            else if (CurrentFloor > target)
            {
                await MoveDownAsync();
            }
            else
            {
                Logger.Log($"Elevator {Id} stopped at floor {CurrentFloor} (boarding/unboarding).");
                await Task.Delay(10000); // 10 sec stop for passengers
                _requests.Dequeue();

                // Update direction
                if (_requests.Count > 0)
                {
                    int next = _requests.Peek();
                    Direction = next > CurrentFloor ? Direction.Up : Direction.Down;
                }
                else
                {
                    Direction = Direction.Idle;
                }
            }
        }

        private async Task MoveUpAsync()
        {
            await Task.Delay(10000); // 10 sec travel
            CurrentFloor++;
            Logger.Log($"Elevator {Id} moved up to floor {CurrentFloor}.");
        }

        private async Task MoveDownAsync()
        {
            await Task.Delay(10000); // 10 sec travel
            CurrentFloor--;
            Logger.Log($"Elevator {Id} moved down to floor {CurrentFloor}.");
        }
    }
}
