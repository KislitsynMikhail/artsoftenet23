namespace ConsoleApp1.Synchronizations;

public static class SynchronizationExample
{
    private static ILocker _locker;

    private static void SetLocker()
    {
        //_locker = new InterLockedLocker();
        //_locker = new MonitorLocker();
        //_locker = new MutexLocker();
        //_locker = new SemaphoreLocker();
        _locker = new SemaphoreSlimLocker();
    }
    
    public static void Do()
    {
        SetLocker();
        
        var t1 = new Thread(Method1);
        var t2 = new Thread(Method1);

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();
    }

    private static void Method1()
    {
        _locker.Wait();

        try
        {
            Console.WriteLine($"{Environment.CurrentManagedThreadId}-{nameof(Method1)}");
            Method2();
        }
        finally
        {
            _locker.Release();
        }
    }

    private static void Method2()
    {
        _locker.Wait();

        try
        {
            Console.WriteLine($"{Environment.CurrentManagedThreadId}-{nameof(Method2)}");
        }
        finally
        {
            _locker.Release();
        }
    }
}