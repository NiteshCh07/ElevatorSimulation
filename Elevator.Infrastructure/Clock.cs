public sealed class SystemClock : IClock
{
    public ValueTask DelayAsync(int milliseconds, CancellationToken ct) => new ValueTask(Task.Delay(milliseconds, ct));
}
