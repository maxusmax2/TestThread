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
        _workThread.Start();
    }

    public void AddAction(Action action, CancellationToken cancellationToken = default)
    {
        _workQueue.Enqueue(new ActionWrapper(action, cancellationToken));
    }

    private void Worker()
    {
        while (!_ct.IsCancellationRequested)
        {
            if (_workQueue.TryDequeue(out var task))
            {
                if (!task.CancellationToken.IsCancellationRequested) 
                {
                    task.Action.Invoke();
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

        public ActionWrapper(Action action, CancellationToken cancellationToken)
        {
            Action = action;
            CallbackId = Guid.NewGuid();
            CancellationToken = cancellationToken;
        }
    }
}
