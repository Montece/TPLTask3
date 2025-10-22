namespace TPLTask3;

internal sealed class Philosopher : IDisposable
{
    private static readonly TimeSpan _maxThinkTime = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan _deadlockTimeout = TimeSpan.FromMilliseconds(100);

    private readonly Random _random = new();
    private readonly BowlOfSpaghetti _bowlOfSpaghetti;
    private readonly Fork _leftFork;
    private readonly Fork _rightFork;

    private CancellationTokenSource? _cancellation;
    private Thread? _dinnerThread;
    private bool _disposed;

    public Philosopher(BowlOfSpaghetti bowlOfSpaghetti, Fork leftFork, Fork rightFork)
    {
        _bowlOfSpaghetti = bowlOfSpaghetti;
        _leftFork = leftFork;
        _rightFork = rightFork;
    }

    public void StartDinner()
    {
        ThrowIfDisposed();

        if (_dinnerThread != null && _cancellation != null)
        {
            return;
        }

        _cancellation ??= new();

        _dinnerThread = new(DinnerCycle);

        _dinnerThread.Start();
    }

    public void StopDinner()
    {
        ThrowIfDisposed();

        if (_dinnerThread == null || _cancellation == null)
        {
            return;
        }

        _cancellation.Cancel();
        
        _dinnerThread.Join();

        _cancellation.Dispose();
        _cancellation = null;
    }

    public void WaitForEndDinner()
    {
        ThrowIfDisposed();

        if (_dinnerThread == null || _cancellation == null)
        {
            return;
        }

        _dinnerThread.Join();

        _cancellation.Dispose();
        _cancellation = null;
    }

    private void DinnerCycle(object? arguments)
    {
        while (!_bowlOfSpaghetti.IsEmpty && _cancellation != null && !_cancellation.IsCancellationRequested)
        {
            ThrowIfDisposed();

            Dinner();
        }
    }

    private void Dinner()
    {
        ThrowIfDisposed();

        Thinking(_maxThinkTime);

        if (!_leftFork.Capture(_deadlockTimeout))
        {
            return;
        }

        if (!_rightFork.Capture(_deadlockTimeout))
        {
            _leftFork.Release();
            return;
        }

        _bowlOfSpaghetti.Eat();

        _rightFork.Release();
        _leftFork.Release();
    }

    private void Thinking(TimeSpan maxThinkTime)
    {
        ThrowIfDisposed();

        Thread.Sleep(_random.Next((int)maxThinkTime.TotalMilliseconds));
    }

    public void Dispose()
    {
        ThrowIfDisposed();

        _disposed = true;

        _cancellation?.Dispose();
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new InvalidOperationException("Object was disposed.");
        }
    }
}