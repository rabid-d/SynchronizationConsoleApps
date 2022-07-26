/*var rwlock = new ReaderWriterLockSlim();
var fileName = "file.txt";

Task.Run(() =>
{
    rwlock.EnterWriteLock();
    Console.WriteLine($"Enter write lock: {GetDateTime()}");
    using (var writetext = new StreamWriter(fileName))
    {
        for (int i = 0; i < 1500; i++)
        {
            string text = "";
            for (int j = 0; j < 150; j++)
            {
                text += Guid.NewGuid();
            }
            writetext.WriteLine(GetDateTime() + " " + text);
            Thread.Sleep(1);
        }
    }
    Console.WriteLine($"Exit write lock: {GetDateTime()}\n");
    rwlock.ExitWriteLock();
});

Thread.Sleep(500);

foreach (var i in Enumerable.Range(1, 5))
{
    Task.Run(() =>
    {
        rwlock.EnterReadLock();
        Console.WriteLine("Enter read lock: " + GetDateTime());
        string text = File.ReadAllText(fileName);
        File.WriteAllText($"file{i}.txt", text);
        Console.WriteLine("Exit read lock: " + GetDateTime());
        rwlock.ExitReadLock();
    });
}

string GetDateTime() => DateTime.UtcNow.ToString("HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);

Console.ReadKey();*/


/*  Semaphore version.  */

/*var semaphore = new Semaphore(1, 5);
var fileName = "file.txt";

var writeTask = Task.Run(() =>
{
    semaphore.WaitOne();
    Console.WriteLine($"Enter write lock: {GetDateTime()}");
    using (var writetext = new StreamWriter(fileName))
    {
        for (int i = 0; i < 500; i++)
        {
            string text = "";
            for (int j = 0; j < 150; j++)
            {
                text += Guid.NewGuid();
            }
            writetext.WriteLine(GetDateTime() + " " + text);
            Thread.Sleep(1);
        }
    }
    Console.WriteLine($"Exit write lock: {GetDateTime()}\n");
    semaphore.Release(5);
});

Thread.Sleep(500);

foreach (var i in Enumerable.Range(1, 5))
{
    Task.Run(() =>
    {
        semaphore.WaitOne();
        Console.WriteLine("Enter read lock: " + GetDateTime());
        string text = File.ReadAllText(fileName);
        File.WriteAllText($"file{i}.txt", text);
        Console.WriteLine("Exit read lock: " + GetDateTime());
        semaphore.Release();
    });
}

string GetDateTime() => DateTime.UtcNow.ToString("HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);

Console.ReadKey();*/


/*  ManualResetEvent version.  */

using System.Globalization;
using System.Text;

ManualResetEvent mre = new(false);
string fileName = "file.txt";
StringBuilder sb = new();

Task.Run(() =>
{
    Console.WriteLine($"Start writing thread: {GetDateTime()}");
    using (StreamWriter writetext = new (fileName))
    {
        for (int i = 0; i < 500; i++)
        {
            sb.Clear();
            for (int j = 0; j < 150; j++)
            {
                sb.Append(Guid.NewGuid());
            }
            writetext.WriteLine($"{GetDateTime()} {sb}");
            Thread.Sleep(1);
        }
    }
    Console.WriteLine($"End writing thread: {GetDateTime()}");
    mre.Set();
    mre.Reset();
});

foreach (int i in Enumerable.Range(1, 5))
{
    Task.Run(() =>
    {
        mre.WaitOne();
        Console.WriteLine($"Enter read lock: {GetDateTime()}");
        string text = File.ReadAllText(fileName);
        File.WriteAllText($"file{i}.txt", text);
        Console.WriteLine($"Exit read lock: {GetDateTime()}");
    });
}

string GetDateTime() => DateTime.UtcNow.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);

Console.ReadKey();
