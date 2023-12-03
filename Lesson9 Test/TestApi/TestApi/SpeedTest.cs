using System.Threading;
using Xunit;

namespace TestApi;

[Collection("1")]
public class SpeedTest1
{
    [Fact]
    public void Test1()
    {
        Thread.Sleep(3000);
    }
}

[Collection("1")]
public class SpeedTest2
{
    [Fact]
    public void Test2()
    {
        Thread.Sleep(3000);
        
    }
}