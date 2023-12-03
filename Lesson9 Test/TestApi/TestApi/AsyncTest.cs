using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;

namespace TestApi;

public class AsyncTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AsyncTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public void Members_Of_Async_Execution_Should_Have_Async_Prefix()
    {
        var coreAssembly = Assembly.Load("Api");

        var valid = true;
        _testOutputHelper.WriteLine("start");

        foreach (var type in coreAssembly.DefinedTypes)
        {
            var methods = type.GetMembers().Where(x => x.MemberType == MemberTypes.Method).OfType<MethodInfo>();

            foreach (var method in methods)
            {
                if (method.ReturnType.BaseType?.Name == "Task" && !method.Name.EndsWith("Async"))
                {
                    _testOutputHelper.WriteLine($"[Task] Name of async method in {method.DeclaringType.FullName} without 'Async' postfix and named as {method.Name}");
                    valid = false;
                }

                if (method.ReturnType.BaseType?.Name.StartsWith("ValueTask`") == true &&
                    !method.Name.EndsWith("Async"))
                {
                    _testOutputHelper.WriteLine(
                        $"[ValueTask] Name of async method in {method.DeclaringType.FullName} without 'Async' postfix and named as {method.Name}");
                }
            }
        }

        Assert.True(valid);
    }
}