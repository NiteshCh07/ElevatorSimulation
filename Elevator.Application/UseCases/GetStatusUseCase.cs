using Elevator.Application.DTOs;
using Elevator.Domain;

namespace Elevator.Application.UseCases;
public sealed class GetStatusUseCase
{
    private readonly IElevatorRepository _repo;
    public GetStatusUseCase(IElevatorRepository repo) => _repo = repo;

    public IReadOnlyList<ElevatorStatusDto> Execute()
    {
        return _repo.GetAll()
            .Select(e => new ElevatorStatusDto(e.Id, e.CurrentFloor, e.Direction.ToString(), e.PendingStopsCount))
            .ToList();
    }
}
