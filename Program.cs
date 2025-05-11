using static System.Console;
using TestThread;
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine(await (await await false && await true)); 
    }
}