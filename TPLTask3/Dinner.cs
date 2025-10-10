namespace TPLTask3;

internal sealed class Dinner : IDisposable
{
    public bool IsStarted { get; private set; }

    private readonly ILogger _logger;
    private readonly List<Fork> _forks = [];

    private Philosopher[]? _philosophers;

    public Dinner(ILogger logger)
    {
        _logger = logger;
    }

    public void Start()
    {
        if (IsStarted)
        {
            return;
        }

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
        if (!IsStarted)
        {
            return;
        }

        foreach (var philosopher in _philosophers)
        {
            philosopher.WaitForEndDinner();
        }
    }

    public void Stop()
    {
        if (!IsStarted)
        {
            return;
        }


    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}