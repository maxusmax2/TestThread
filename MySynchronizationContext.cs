using System.Collections.Concurrent;

namespace TestThread;

public class MySynchronizationContext : SynchronizationContext
{
    private readonly ConcurrentQueue<HandlerWithData> _queue =
        new ConcurrentQueue<HandlerWithData>();

    private readonly Thread _thread;

    public MySynchronizationContext()
    {
        _thread = new Thread(RunOnCurrentThread)
        {
            IsBackground = true
        };
        _thread.Start();
    }

    public override void Post(SendOrPostCallback callback, object state)
    {
        _queue.Enqueue(new(callback, state));
    }

    public override void Send(SendOrPostCallback callback, object state)
    {
        if (Thread.CurrentThread == _thread)
        {
            callback(state); // Run inline if we're already on the thread
        }
        else
        {
            var done = new ManualResetEvent(false);
            Exception exception = null;

            Post(s =>
            {
                try
                {
                    callback(s);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                finally
                {
                    done.Set();
                }
            }, state);

            done.WaitOne();

            if (exception != null)
                throw exception;
        }
    }

    private void RunOnCurrentThread()
    {
        SetSynchronizationContext(this);
        while (true) 
        {
            if (_queue.Count > 0)
            {
                if (_queue.TryDequeue(out HandlerWithData handler))
                {
                    handler.callback(handler.state);
                }
            }
            else
                Thread.Sleep(1);
        }
    }

    private record HandlerWithData(SendOrPostCallback callback, object? state);
}
