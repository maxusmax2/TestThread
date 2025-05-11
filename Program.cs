using static System.Console;
using TestThread;
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine(await (await await false && await true));

        MyAwaiter me = new MyAwaiter();
        WriteLine($"Run {Thread.CurrentThread.ManagedThreadId}");
        _ = Task.Run(() => 
        {
            Thread.Sleep(1000);
            WriteLine($"SetCompleted {Thread.CurrentThread.ManagedThreadId}");
            me.SetCompeleted();
        });
        await me;
        WriteLine($"After await {Thread.CurrentThread.ManagedThreadId}");
        ReadLine();
    }
}