Console.Write("Enter number of racing threads: ");
var n = int.Parse(Console.ReadLine());
var rand = new Random();
var roundTurns = new List<int>();
var lines = new string[n];
var tokenSource = new CancellationTokenSource();
CancellationToken ct = tokenSource.Token;
Console.Clear();

foreach (var i in Enumerable.Range(0, n))
{
    Task.Run(() =>
    {
        while (true)
        {
            Monitor.Enter(typeof(Console));
            if (ct.IsCancellationRequested)
            {
                break;
            }
            if (roundTurns.Contains(i))
            {
                if (roundTurns.Count == n)
                {
                    roundTurns.Clear();
                }
                else
                {
                    Monitor.Exit(typeof(Console));
                    continue;
                }
            }
            Console.SetCursorPosition(lines[i]?.Length ?? 0, i);
            int randValue = rand.Next(1, 3);
            for (int j = 0; j < randValue; j++)
            {
                Console.Write('-');
                lines[i] += '-';
            }
            roundTurns.Add(i);
            if (lines[i].Length >= 50)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine($"Thread #{i + 1} is the winner!");
                tokenSource.Cancel();
            }
            Thread.Sleep(10);
            Monitor.Exit(typeof(Console));
        }
    }, tokenSource.Token);
}

Console.ReadKey();
