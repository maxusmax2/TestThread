using TestThread;
var context = new MyCustomSynchronizationContext();

var thread = new Thread(() =>
{
    // Устанавливаем контекст для этого потока
    SynchronizationContext.SetSynchronizationContext(context);

    // Запускаем loop обработки задач
    context.Run(); // <<< ВОТ ЗДЕСЬ мы явно его вызываем
    Console.WriteLine("Thread завершён");
});

context.Post((x) => Console.WriteLine("Work in sync context" + x), 42);
context.Post((x) => Console.WriteLine("Work in sync context" + x), 73);

thread.Start();

thread.Join();
