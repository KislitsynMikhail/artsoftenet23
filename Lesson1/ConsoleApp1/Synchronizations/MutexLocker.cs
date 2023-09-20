namespace ConsoleApp1.Synchronizations;

public class MutexLocker : ILocker
{
    private readonly Mutex _mutex = new();
    
    public void Wait()
    {
        _mutex.WaitOne();
    }

    public void Release()
    {
        _mutex.ReleaseMutex();
    }
}