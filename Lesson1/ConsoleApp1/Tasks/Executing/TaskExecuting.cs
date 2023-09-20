namespace ConsoleApp1.Tasks.Executing;

public class TaskExecuting
{
    private readonly SyncExecuting _syncExecuting = new();
    
    public Task Doo()
    {
        var task = new Task(_syncExecuting.Doo);
        task.Start();

        return task;
    }

    public Task Foo()
    {
        var task = new Task(_syncExecuting.Foo);
        task.Start();
        
        return task;
    }
}