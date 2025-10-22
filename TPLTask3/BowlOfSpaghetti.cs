namespace TPLTask3;

internal sealed class BowlOfSpaghetti
{
    public bool IsEmpty => _currentServingsCount <= 0;
    
    private int _currentServingsCount;

    public BowlOfSpaghetti(int servingsCount)
    {
        _currentServingsCount = servingsCount;
    }

    public void Eat()
    {
        if (_currentServingsCount > 0)
        {
            _currentServingsCount--;
        }
    }
}