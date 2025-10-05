using Elevator.Application.UseCases;
using Elevator.Domain;
using Elevator.Infrastructure;
public class ProcessElevatorRequestTests
{
    [Fact]
    public void AssignsClosestIdleElevator()
    {
        var repo = new InMemoryElevatorRepository(count: 3, initialFloor: 1);
        // move car 1 to floor 10 (simulate)
        var car1 = repo.GetById(1)!;
        while (car1.CurrentFloor < 10) car1.MoveOneFloorTowards(10);

        var dispatcher = new SimpleConsoleDispatcher();
        var useCase = new ProcessElevatorRequestUseCase(repo, dispatcher);

        var request = new ElevatorRequest(2, 8); // source floor 2
        useCase.Handle(request);

        // closest idle elevator should be car 2 or 3 (on floor 1). They should have pending stops > 0
        var assigned = repo.GetAll().Where(e => e.PendingStopsCount > 0).ToList();
        Assert.True(assigned.Count > 0);
        Assert.DoesNotContain(assigned, e => e.Id == 1); // car 1 was far away; at least one other got the job
    }
}
