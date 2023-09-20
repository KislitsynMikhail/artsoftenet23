namespace ConsoleApp1.Synchronizations;

public interface ILocker
{
    void Wait();

    void Release();
}