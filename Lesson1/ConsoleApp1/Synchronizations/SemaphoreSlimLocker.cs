namespace ConsoleApp1.Synchronizations;

public class SemaphoreSlimLocker : ILocker
{
    private SemaphoreSlim _sem = new(3, 3);
    
    public void Wait()
    {
        _sem.Wait();
    }

    public void Release()
    {
        _sem.Release();
    }
}