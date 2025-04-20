namespace TestThread;

public class MyCustomSynchronizationContext : SynchronizationContext
{
    public Queue<(SendOrPostCallback, object)> Callbacks { get; private set; } = new Queue<(SendOrPostCallback, object)>();
    public void Post(SendOrPostCallback d, object? state)
    {
        Callbacks.Enqueue((d, state));
    }

    public void Run() 
    {
        while (true) 
        {
            if (Callbacks.Count > 0) 
            {
                var (d,state) = Callbacks.Dequeue();
                d(state);
            }
            else
                Thread.Sleep(100);
        }
    }
}
