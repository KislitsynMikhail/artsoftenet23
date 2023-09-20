using ConsoleApp1.Tasks.Executing;

namespace ConsoleApp1.Tasks;

public class TaskExample
{
    public static async Task Method()
    {
        //await Difference();

        var example = new TaskExample();
        
        Task task;
        //task = example.VoidAsync();
        //task = example.TaskAsync();
        task = example.TaskGroupAsync();

        await task;
    }

    private static async Task Difference()
    {
        var stopWatch = new System.Diagnostics.Stopwatch();
        
        /*
        var syncExecuting = new SyncExecuting();
        stopWatch.Start();
        
        syncExecuting.Doo();
        Console.WriteLine();
        
        syncExecuting.Foo();
        Console.WriteLine();
        
        stopWatch.Stop();
        Console.WriteLine($"sync : {stopWatch.ElapsedMilliseconds} mc");
        Console.WriteLine();
        
        ///////////////////////////////////////////////////////////////////////

        var threadExecuting = new ThreadExecuting();
        stopWatch.Restart();
        
        var t1 = threadExecuting.Doo();
        
        var t2 = threadExecuting.Foo();
        t1.Join();
        t2.Join();
        Console.WriteLine();
        
        stopWatch.Stop();
        Console.WriteLine($"thread : {stopWatch.ElapsedMilliseconds} mc");
        Console.WriteLine();
        
        /////////////////////////////////////////////////////////////////////////

        var taskExecuting = new TaskExecuting();
        stopWatch.Restart();
        
        var task1 = taskExecuting.Doo();
        var task2 = taskExecuting.Foo();
        task1.Wait();
        task2.Wait();
        Console.WriteLine();
        
        stopWatch.Stop();
        Console.WriteLine($"task : {stopWatch.ElapsedMilliseconds} mc");
        Console.WriteLine();*/
        
        ///////////////////////////////////////////////////////////////////////

        /*var asyncExecuting = new AsyncExecuting();
        stopWatch.Restart();
        
        Console.WriteLine($"{Environment.CurrentManagedThreadId}");
        var task3 = asyncExecuting.Doo();
        Console.WriteLine($"{Environment.CurrentManagedThreadId}");
        Console.WriteLine();
        
        Console.WriteLine($"{Environment.CurrentManagedThreadId}");
        await asyncExecuting.Foo();
        
        Console.WriteLine();
        Console.WriteLine($"{Environment.CurrentManagedThreadId}");
        await task3;
        Console.WriteLine($"{Environment.CurrentManagedThreadId}");
        Console.WriteLine();
        
        stopWatch.Stop();
        Console.WriteLine($"async : {stopWatch.ElapsedMilliseconds} mc");
        Console.WriteLine();*/
    }

    private async Task TaskAsync()
    {
        try
        {
            await PrintAsync("Hello");
            await PrintAsync("Hi");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        async Task PrintAsync(string message)
        {
            // если длина строки меньше 3 символов, генерируем исключение
            if (message.Length < 3)
            {
                var error = $"Invalid string length: {message.Length}";
                throw new ArgumentException(error);
            }
            await Task.Delay(100); // имитация продолжительной операции
            Console.WriteLine(message);
        }
    }

    private async Task VoidAsync()
    {
        try
        {
            PrintAsync("Hello");
     
            // здесь программа сгенерирует исключение и аварийно остановится
            PrintAsync("Hi");      
            await Task.Delay(1000); // ждем завершения задач
        }
        catch (Exception ex)    // исключение НЕ будет обработано
        {
            Console.WriteLine(ex.Message);
        }
 
        async void PrintAsync(string message)
        {
            // если длина строки меньше 3 символов, генерируем исключение
            if (message.Length < 3)
            {
                var error = $"Invalid string length: {message.Length}";
                throw new ArgumentException(error);
            }
            await Task.Delay(100); // имитация продолжительной операции
            Console.WriteLine(message);
        }
    }

    private async Task TaskGroupAsync()
    {
        // определяем и запускаем задачи
        var task1 = PrintAsync("H");
        var task2 = PrintAsync("Hi");
        var allTasks = Task.WhenAll(task1, task2);
        try
        {
            await allTasks;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            Console.WriteLine($"IsFaulted: {allTasks.IsFaulted}");
            if (allTasks.Exception is not null)
            {
                foreach (var exception in allTasks.Exception.InnerExceptions)
                {
                    Console.WriteLine($"InnerException: {exception.Message}");
                }
            }
        }
 
        async Task PrintAsync(string message)
        {
            // если длина строки меньше 3 символов, генерируем исключение
            if (message.Length < 3)
            {
                throw new ArgumentException($"Invalid string: {message}");
            }
            await Task.Delay(1000); // имитация продолжительной операции
            Console.WriteLine(message);
        }
    }
}