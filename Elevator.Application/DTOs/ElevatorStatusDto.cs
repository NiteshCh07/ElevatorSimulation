namespace Elevator.Application.DTOs;
public record ElevatorStatusDto(int Id, int CurrentFloor, string Direction, int PendingStops);
