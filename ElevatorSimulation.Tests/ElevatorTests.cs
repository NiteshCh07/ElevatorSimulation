using ElevatorSimulation.Models;

namespace ElevatorSimulation.Tests
{
    public class ElevatorTests
    {
        [Fact]
        public async Task Elevator_ShouldMoveUp_WhenRequestIsAbove()
        {
            var elevator = new Elevator(1, startFloor: 1);
            elevator.AddRequest(3);

            await elevator.StepAsync(); // move to floor 2
            Assert.Equal(2, elevator.CurrentFloor);

            await elevator.StepAsync(); // move to floor 3
            Assert.Equal(3, elevator.CurrentFloor);
        }

        [Fact]
        public async Task Elevator_ShouldMoveDown_WhenRequestIsBelow()
        {
            var elevator = new Elevator(1, startFloor: 5);
            elevator.AddRequest(3);

            await elevator.StepAsync(); // move to 4
            Assert.Equal(4, elevator.CurrentFloor);

            await elevator.StepAsync(); // move to 3
            Assert.Equal(3, elevator.CurrentFloor);
        }

        [Fact]
        public async Task Elevator_ShouldBeIdle_WhenNoRequests()
        {
            var elevator = new Elevator(1, startFloor: 5);

            await elevator.StepAsync(); // no requests
            Assert.Equal(Direction.Idle, elevator.Direction);
        }

        [Fact]
        public async Task Elevator_ShouldStopAndClearRequest_WhenArrivesAtTarget()
        {
            var elevator = new Elevator(1, startFloor: 2);
            elevator.AddRequest(2); // already at target

            await elevator.StepAsync();

            Assert.False(elevator.HasRequests);
            Assert.Equal(Direction.Idle, elevator.Direction);
        }
    }
}
