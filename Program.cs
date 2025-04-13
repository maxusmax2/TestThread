using TestThread;

var taskLoop = new TaskLoop
{
    A = () => Console.WriteLine($"After delay {Thread.CurrentThread.ManagedThreadId}"),
    Max = 5
};
Console.WriteLine($"Hello World {Thread.CurrentThread.ManagedThreadId}");
taskLoop.Run();
taskLoop.Task.Wait();

var threadPool = new MyThreadPool();

for (var i = 0; i < 100; i++)
{
    var f = threadPool.AddAction(async () =>
    {
        i++;
        await MyTask.Delay(5000);
        Console.WriteLine(i);
    }, CancellationToken.None);
    f.Finished += (o, a) => { Console.WriteLine($"Done {i}"); }; ;
}
Console.WriteLine("Hello World!");
Thread.Sleep(1000);
Console.ReadLine();

var executionId = Guid.NewGuid();
var cts = new CancellationTokenSource();
var ct = cts.Token;
AsyncLocal<Guid> local = new()
{
    Value = executionId
};
cts.Cancel();
Console.WriteLine($"Main thread ExecutionID: {local.Value}");
ThreadPool.UnsafeQueueUserWorkItem(_ =>
{
    ct.ThrowIfCancellationRequested();
    Console.WriteLine($"New1 thread ExecutionID: {local.Value}");
}, null);
ThreadPool.QueueUserWorkItem(_ =>
{
    ct.ThrowIfCancellationRequested();
    Console.WriteLine($"New2 thread ExecutionID: {local.Value}");
}, null);


Console.ReadKey();