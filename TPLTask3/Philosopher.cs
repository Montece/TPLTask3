namespace TPLTask3;

internal sealed class Philosopher : IDisposable
{
    private static readonly TimeSpan MaxThinkTime = TimeSpan.FromSeconds(1);

    private readonly Random _random = new();
    private readonly BowlOfSpaghetti _bowlOfSpaghetti;
    private readonly Fork _leftFork;
    private readonly Fork _rightFork;

    private CancellationTokenSource? _cancellation;
    private Thread? _dinnerThread;

    public Philosopher(BowlOfSpaghetti bowlOfSpaghetti, Fork leftFork, Fork rightFork)
    {
        _bowlOfSpaghetti = bowlOfSpaghetti;
        _leftFork = leftFork;
        _rightFork = rightFork;
    }

    public void StartDinner()
    {
        if (_dinnerThread != null && _cancellation != null)
        {
            return;
        }

        _cancellation ??= new();

        _dinnerThread = new Thread(DinnerCycle);

        _dinnerThread.Start();
    }

    public void StopDinner()
    {
        if (_dinnerThread == null || _cancellation == null)
        {
            return;
        }

        _cancellation.Cancel();
        
        _dinnerThread.Join();

        _cancellation.Dispose();
        _cancellation = null;
    }

    private void DinnerCycle(object? arguments)
    {
        while (!_bowlOfSpaghetti.IsEmpty && (_cancellation != null && !_cancellation.IsCancellationRequested))
        {
            Dinner();
        }
    }

    private void Dinner()
    {
        Thinking(MaxThinkTime);
        _leftFork.Capture();
        _rightFork.Capture();
        _bowlOfSpaghetti.Eat();
        _rightFork.Release();
        _leftFork.Release();
    }

    private void Thinking(TimeSpan maxThinkTime)
    {
        Thread.Sleep(_random.Next((int)maxThinkTime.TotalMilliseconds));
    }

    public void Dispose()
    {
        _cancellation?.Dispose();
    }
}