namespace TPLTask3;

internal sealed class Dinner : IDisposable
{
    private bool IsStarted { get; set; }

    private readonly ILogger _logger;
    private readonly List<Fork> _forks = [];

    private Philosopher[]? _philosophers;
    private bool _disposed;

    public Dinner(ILogger logger)
    {
        _logger = logger;
    }

    public void Start()
    {
        ThrowIfDisposed();

        if (IsStarted)
        {
            return;
        }

        IsStarted = true;

        _philosophers = new Philosopher[5];

        _forks.Clear();

        var rightFork = new Fork();
        var firstFork = rightFork;

        for (var i = 0; i < _philosophers.Length; i++)
        {
            var leftFork = i == _philosophers.Length - 1 ? firstFork : new();
            var bowlOfSpaghetti = new BowlOfSpaghetti(5);

            _philosophers[i] = new(bowlOfSpaghetti, leftFork, rightFork);

            if (!_forks.Contains(leftFork))
            {
                _forks.Add(leftFork);
            }

            if (!_forks.Contains(rightFork))
            {
                _forks.Add(rightFork);
            }

            rightFork = leftFork;
        }

        foreach (var philosopher in _philosophers)
        {
            philosopher.StartDinner();
        }

        _logger.Write("Dinner started");
    }

    public void WaitForEnd()
    {
        ThrowIfDisposed();

        if (!IsStarted)
        {
            return;
        }

        foreach (var philosopher in _philosophers)
        {
            philosopher.WaitForEndDinner();
        }

        _logger.Write("Dinner ended");
    }

    public void Stop()
    {
        ThrowIfDisposed();

        if (!IsStarted)
        {
            return;
        }

        IsStarted = false;

        foreach (var philosopher in _philosophers)
        {
            philosopher.StopDinner();
        }
    }

    private void Dispose(bool disposing)
    {
        ThrowIfDisposed();

        _disposed = true;

        if (disposing)
        {
            if (_philosophers == null)
            {
                foreach (var philosopher in _philosophers)
                {
                    philosopher.Dispose();
                }
            }

            foreach (var philosopher in _philosophers)
            {
                philosopher.Dispose();
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    ~Dinner()
    {
        Dispose(false);
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new InvalidOperationException("Object was disposed.");
        }
    }
}