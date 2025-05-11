using System.Runtime.CompilerServices;

namespace TestThread;

public static class BoolAwaiterExtensions
{
    public static BoolAwaiter GetAwaiter(this bool b)
    {
        return new BoolAwaiter(b);
    }
}
public class BoolAwaiter : INotifyCompletion
{
    private bool _value;
    public BoolAwaiter(bool value)
    {
        _value = value;
    }

    public bool IsCompleted { get { return true; } }
    public void OnCompleted(Action action)
    {
        return;
    }
    public void UnsafeOnCompleted(Action action)
    {
        return;
    }

    public bool GetResult()
    {
        return _value;
    }
}