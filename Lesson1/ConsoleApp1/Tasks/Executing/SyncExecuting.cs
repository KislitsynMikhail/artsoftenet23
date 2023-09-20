namespace ConsoleApp1.Tasks.Executing;

public class SyncExecuting
{
    public void Doo()
    {
        PrintTenTime("#");
    }

    public void Foo()
    {
        PrintTenTime("*");
    }

    private void PrintTenTime(string str)
    {
        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(10);
            Console.Write(str);
        }
    }
}