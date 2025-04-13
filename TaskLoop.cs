namespace TestThread;

public class TaskLoop
{
    private TaskCompletionSource _tcs;

    private int _count = 0;
    private Timer _timer;

    public Action A;
    public int Max;
    public Task Task { get
        {
            return _tcs.Task;
        }
    }

    public TaskLoop() 
    {
        _tcs = new();
    }

    public void Run() 
    {
        _timer = new Timer(TimerHanlder, null, 0, 1000);
    }

    private void TimerHanlder(object state) 
    {
        _count++;
        A();
        if (_count >= Max) 
        {
            _tcs.TrySetResult();
            _timer.Dispose();
        }
    }
}
