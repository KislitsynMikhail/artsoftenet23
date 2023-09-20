namespace ConsoleApp1.Tasks.Executing;

public class ThreadExecuting
{
    private readonly SyncExecuting _syncExecuting = new();
    
    public Thread Doo()
    {
        var thread = new Thread(_syncExecuting.Doo);
        thread.Start();

        return thread;
    }

    public Thread Foo()
    {
        var thread = new Thread(_syncExecuting.Foo);
        thread.Start();

        return thread;
    }
}