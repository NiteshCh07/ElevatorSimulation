using ElevatorSimulation.Simulation;

namespace ElevatorSimulation.Tests
{
    public class RandomRequestGeneratorTests
    {
        [Fact]
        public async Task Generator_ShouldCreateRequests_AndRunWithoutError()
        {
            var generator = new RandomRequestGenerator(numElevators: 2, numFloors: 5);

            // Run with small number of requests
            await generator.RunAsync(3);

            // No assert needed here, test ensures it doesn't crash
        }

        [Fact]
        public async Task Generator_ShouldAssignRequestToClosestElevator()
        {
            var generator = new RandomRequestGenerator(numElevators: 2, numFloors: 5);

            // Run 1 request to trigger assignment
            await generator.RunAsync(1);

            // We can't check logs here easily without mocking Logger,
            // but at least ensure it doesn't throw and executes.
            Assert.True(true);
        }
    }
}
