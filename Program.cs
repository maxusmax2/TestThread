using TestThread;

var context = new SingleThreadSynchronizationContext();
SynchronizationContext.SetSynchronizationContext(context);

var task = Task.Run(async () =>
{
    Console.WriteLine($"Start: {Thread.CurrentThread.ManagedThreadId}");
    await Task.Delay(500);
    Console.WriteLine($"After delay: {Thread.CurrentThread.ManagedThreadId}");
});

task.Wait();
