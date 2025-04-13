using System.Collections.Concurrent;

namespace TestThread;

public class MyThreadPool
{
    private readonly ConcurrentQueue<ActionWrapper> _workQueue;
    private readonly CancellationToken _ct;
    private readonly Thread _workThread;
    public MyThreadPool(CancellationToken cancellationToken = default)
    {
        _workQueue = new ConcurrentQueue<ActionWrapper>();
        _ct = cancellationToken;
        _workThread = new Thread(Worker);
        _workThread.IsBackground = false;
        _workThread.Start();
    }

    public Future AddAction(Action action, CancellationToken cancellationToken = default)
    {
        var future = new Future();
        _workQueue.Enqueue(new ActionWrapper(action, future, cancellationToken));
        return future;
    }

    private void Worker()
    {
        while (!_ct.IsCancellationRequested)
        {
            if (_workQueue.TryDequeue(out var task))
            {
                if (!task.CancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"Action start. Id = {task.CallbackId}");
                    task.Action.Invoke();
                    Console.WriteLine($"Action finished. Id = {task.CallbackId}");
                    task.Future.Come(this, EventArgs.Empty);
                }
            }
            else
            {
                Thread.Sleep(1);
                continue;
            }
        }
    }

    private class ActionWrapper
    {
        public Action Action { get; init; }
        public Guid CallbackId { get; init; }
        public CancellationToken CancellationToken { get; init; }
        public Future Future;

        public ActionWrapper(Action action, Future future, CancellationToken cancellationToken)
        {
            Action = action;
            CallbackId = Guid.NewGuid();
            CancellationToken = cancellationToken;
            Future = future;
        }
    }
    public class Future
    {
        public event EventHandler<EventArgs> Finished;

        internal void Come(object sender, EventArgs args)
        {
            Finished.Invoke(sender, args);
        }
    }
}
