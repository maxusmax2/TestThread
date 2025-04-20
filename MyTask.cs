namespace TestThread;

public class MyTask
{
    public static Task Delay(int delay) 
    {
        var tcs = new TaskCompletionSource();
        var timer = new Timer((x) => { tcs.TrySetResult();  },null, TimeSpan.Zero, TimeSpan.MaxValue);
        return tcs.Task;
    }
}
