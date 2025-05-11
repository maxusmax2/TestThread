using System.Runtime.CompilerServices;

namespace TestThread;

public class MyAwaiter : INotifyCompletion, ICriticalNotifyCompletion
{
    private bool _completed;
    private TaskCompletionSource _tcs = new();
    public bool IsCompleted
    {
        get
        {
            return _completed;
        }
    }
    public void OnCompleted(Action continuation)
    {
        _tcs.Task.GetAwaiter().OnCompleted(continuation);
    }

    public void SetCompeleted()
    {
        _tcs.SetResult();
        _completed = true;
    }

    public MyAwaiter GetAwaiter()
    {
        return this;
    }

    public void GetResult()
    {
        return;
    }

    public void UnsafeOnCompleted(Action continuation)
    {
        throw new NotImplementedException();
    }
}
