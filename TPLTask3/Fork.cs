namespace TPLTask3;

internal sealed class Fork : IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1);

    public void Capture()
    {
        _semaphore.Wait();
    }

    public void Release()
    {
        _semaphore.Release();
    }

    public void Dispose()
    {
        _semaphore.Dispose();
    }
}