using TPLTask3;

using var dinner = new Dinner(new ConsoleLogger());

dinner.Start();
dinner.WaitForEnd();
dinner.Stop();

Console.WriteLine("Press ENTER to exit...");
Console.ReadLine();