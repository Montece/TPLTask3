namespace TPLTask3;

internal sealed class Fork : IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1);

    private bool _disposed;

    public bool Capture(TimeSpan timeout)
    {
        ThrowIfDisposed();

        return _semaphore.Wait(timeout);
    }

    public void Release()
    {
        ThrowIfDisposed();

        _semaphore.Release();
    }

    public void Dispose()
    {
        ThrowIfDisposed();

        _disposed = true;

        _semaphore.Dispose();
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new InvalidOperationException("Object was disposed.");
        }
    }
}