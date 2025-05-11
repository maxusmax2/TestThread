namespace TestThread
{
    public static class Helper
    {
        public static Task<TResult> WithCancellation<TResult>(this Task<TResult> task, CancellationToken ct)
        {
            return task.WaitAsync(ct);
        }


        public static async Task<TResult[]> WhenAllOrError<TResult>(params Task<TResult>[] tasks)
        {
            var taskList = tasks.ToList();
            var completedTasks = new List<Task>();

            while (taskList.Any())
            {
                var completedTask = await Task.WhenAny(taskList);
                taskList.Remove(completedTask);
                completedTasks.Add(completedTask);

                if (completedTask.IsFaulted)
                {
                    throw completedTask.Exception?.Flatten();
                }
            }

            return await Task.WhenAll(tasks);
        }
    }
}
