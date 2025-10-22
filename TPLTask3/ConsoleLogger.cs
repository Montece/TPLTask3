namespace TPLTask3;

internal sealed class ConsoleLogger : ILogger
{
    public void Write(object message)
    {
        Console.WriteLine(message);
    }
}