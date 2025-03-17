using TestThread;

var cts = new CancellationTokenSource();
var ct = cts.Token;
var threadPool = new MyThreadPool();

for (var i = 0; i < 100; i++)
{
    var f = threadPool.AddAction(() =>
    {
        i++;
        Console.WriteLine(i);
        Thread.Sleep(1);
    }, cts.Token);
    f.Finished += (o, a) => { Console.WriteLine($"Done {i}"); }; ;
}
Console.WriteLine("Hello World!");
Thread.Sleep(1000);
cts.Cancel();

Console.ReadLine();