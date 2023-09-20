namespace ConsoleApp1.Synchronizations;

public class MonitorLocker : ILocker
{
    private object _sync = new();
    
    public void Wait()
    {
        Monitor.Enter(_sync);
    }

    public void Release()
    {
        Monitor.Exit(_sync);
    }
}