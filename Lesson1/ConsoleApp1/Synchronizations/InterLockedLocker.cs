namespace ConsoleApp1.Synchronizations;

public class InterLockedLocker : ILocker
{
    private int _sync;
    public void Wait()
    {
        while (true)
        {
            if (Interlocked.CompareExchange(ref _sync, value: 1, comparand: 0) == 0)
            {
                return;
            }
        }
    }

    public void Release()
    {
        _sync = 0;
    }
}