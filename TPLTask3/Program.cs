using TPLTask3;

var philosophers = new Philosopher[5];

var rightFork = new Fork();
var firstFork = rightFork;

for (var i = 0; i < philosophers.Length; i++)
{
    var leftFork = i == philosophers.Length - 1 ? firstFork : new Fork();
    var bowlOfSpaghetti = new BowlOfSpaghetti(5);

    philosophers[i] = new Philosopher(bowlOfSpaghetti, leftFork, rightFork);
    
    rightFork = leftFork;
}

foreach (var philosopher in philosophers)
{
    philosopher.StartDinner();
}

Console.WriteLine("Dinner ended");

Console.WriteLine("Press ENTER to exit...");
Console.ReadLine();