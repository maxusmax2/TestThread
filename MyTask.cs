namespace TestThread;

public class MyTask
{
    public static Task Delay(int delay) 
    {
        var tcs = new TaskCompletionSource();
        var timer = new Timer((x) => tcs.TrySetResult(),null, delay, delay);
        return tcs.Task;
    }
}
