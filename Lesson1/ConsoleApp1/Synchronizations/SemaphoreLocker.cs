namespace ConsoleApp1.Synchronizations;

public class SemaphoreLocker : ILocker
{
    private Semaphore _sem = new(3, 3);
    
    public void Wait()
    {
        _sem.WaitOne();
    }

    public void Release()
    {
        _sem.Release();
    }
}