using ElevatorSimulation.Simulation;

namespace ElevatorSimulation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Elevator Simulation Started!");

            // Create simulation runner with 4 elevators and 10 floors
            var generator = new RandomRequestGenerator(numElevators: 4, numFloors: 10);

            // Run simulation (10 requests demo)
            await generator.RunAsync(10);

            Console.WriteLine("Simulation Finished.");
        }
    }
}
