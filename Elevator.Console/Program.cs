using Elevator.Application.UseCases;
using Elevator.Application.Services;
using Elevator.Domain;
using Elevator.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder()
    .ConfigureServices((ctx, services) =>
    {
        // domain/infrastructure
        services.AddSingleton<IElevatorRepository>(_ => new InMemoryElevatorRepository(count: 4, initialFloor: 1));
        services.AddSingleton<RandomRequestGenerator>();
        services.AddSingleton<IRequestDispatcher, SimpleConsoleDispatcher>();
        services.AddSingleton<IClock, SystemClock>();

        // application
        services.AddTransient<ProcessElevatorRequestUseCase>();
        services.AddTransient<GetStatusUseCase>();
        services.AddSingleton<SimpleScheduler>();

        // infra display
        services.AddSingleton<ConsoleDisplay>();

        // hosted services
        services.AddHostedService<ElevatorHostedService>();
    })
    .UseConsoleLifetime();

await builder.RunConsoleAsync();


// Hosted service to tie pieces together: generate random requests and show status.
public class ElevatorHostedService : IHostedService
{
    private readonly IServiceProvider _services;
    private CancellationTokenSource? _cts;
    private Task? _schedulerTask;

    public ElevatorHostedService(IServiceProvider services) => _services = services;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var ct = _cts.Token;

        // Start scheduler loop
        var scheduler = _services.GetRequiredService<SimpleScheduler>();
        _schedulerTask = Task.Run(() => scheduler.StartAsync(ct), ct);

        // Start request generator loop and display loop
        _ = Task.Run(async () =>
        {
            var randGen = _services.GetRequiredService<RandomRequestGenerator>();
            var process = _services.GetRequiredService<ProcessElevatorRequestUseCase>();
            var display = _services.GetRequiredService<ConsoleDisplay>();

            while (!ct.IsCancellationRequested)
            {
                var req = randGen.Next();
                Console.WriteLine($"[Generator] New: {req.SourceFloor} -> {req.DestinationFloor} ({req.Direction})");
                process.Handle(req);

                // refresh console snapshot
                display.WriteSnapshot();

                // wait 2-6 seconds between requests
                await Task.Delay(2000 + new Random().Next(0, 4000), ct);
            }
        }, ct);

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _cts?.Cancel();
        if (_schedulerTask != null) await _schedulerTask;
    }
}
