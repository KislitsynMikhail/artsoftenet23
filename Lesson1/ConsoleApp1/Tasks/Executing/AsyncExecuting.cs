namespace ConsoleApp1.Tasks.Executing;

public class AsyncExecuting
{
    private readonly TaskExecuting _taskExecuting = new();
    
    public async Task Doo(Stream s)
    {
        await _taskExecuting.Doo();
    }

    public async Task Foo()
    {
        await using var stream = new MemoryStream();

        await Doo(stream);
    }
}