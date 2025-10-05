ElevatorSolution/
│
├── Elevator.Domain/                (class library)
│   ├── Elevator.cs
│   ├── ElevatorRequest.cs
│   ├── ElevatorDirection.cs
│   └── IElevatorRepository.cs
│
├── Elevator.Application/           (class library)
│   ├── DTOs/
│   │   └── ElevatorStatusDto.cs
│   ├── Interfaces/
│   │   ├── IRequestDispatcher.cs
│   │   └── IClock.cs
│   ├── UseCases/
│   │   ├── ProcessElevatorRequestUseCase.cs
│   │   └── GetStatusUseCase.cs
│   └── Services/
│       └── SimpleScheduler.cs
│
├── Elevator.Infrastructure/        (class library)
│   ├── InMemoryElevatorRepository.cs
│   ├── RandomRequestGenerator.cs
│   └── ConsoleDisplay.cs
│
├── Elevator.Console/               (console app)
│   ├── Program.cs
│
└── Elevator.Tests/                 (xUnit tests)
    └── ProcessElevatorRequestTests.cs
