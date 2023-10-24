using System;

namespace Lesson1.Services;

public class FirstService : IService
{
    public int GetRandomNumber()
    {
        var dd = new Random();
        return dd.Next();
    }
}

public interface IService
{
    public int GetRandomNumber();
}