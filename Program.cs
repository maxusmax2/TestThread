using TestThread;

SynchronizationContext.SetSynchronizationContext(new MySynchronizationContext());
Console.WriteLine($"App start, Thread:{Environment.CurrentManagedThreadId}");
var t = Task.Delay(1000);
await t;
Console.WriteLine($"Context - {SynchronizationContext.Current is not null}");
Console.WriteLine($"After await:{Environment.CurrentManagedThreadId}");