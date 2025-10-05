public interface IClock
{
    ValueTask DelayAsync(int milliseconds, CancellationToken ct);
}
