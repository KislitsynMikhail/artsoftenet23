using System.Linq;
using Api;
using Newtonsoft.Json.Linq;
using Xunit;

namespace TestApi;

public class TestWriteToJsonTest
{
    [Theory]
    [InlineData("key")]
    [InlineData("key2")]
    [InlineData("key3")]
    public void Test1_InlineData(string path)
    {
        var data = new JObject()
        {
            {"key", new JObject()}
        };
        var value = 10;
        var splitFormat = path.Split(".").ToArray();
        var res = JObjectHelper.AddToJsonByPath(data, splitFormat, value);

        Assert.Equal(10, res[path]);
    }
    
    [Fact]
    public void Test2_Fact()
    {
        var data = new JObject()
        {
            {"key", new JObject()}
        };
        var value = 10;
        var splitFormat = "key.key.key".Split(".").ToArray();
        var res = JObjectHelper.AddToJsonByPath(data, splitFormat, value);
        Assert.Equal(10, res["key"]["key"]["key"]);
    }
    
    [Theory]
    [InlineData("[0]")]
    public void Test3(string path)
    {
        var data = new JArray();
        var value = 10;
        var splitFormat = path.Split(".").ToArray();
        var res = JObjectHelper.AddToJsonByPath(data, splitFormat, value);
        Assert.Equal(10, res[0]);
    }
    
    [Theory]
    [InlineData("[1]")]
    public void Test4(string path)
    {
        var data = new JArray();
        var value = 10;
        var splitFormat = path.Split(".").ToArray();
        var res = JObjectHelper.AddToJsonByPath(data, splitFormat, value);
        Assert.Equal(10, res[1]);
    }
    
    [Theory]
    [InlineData("[10]")]
    public void Test5(string path)
    {
        var data = new JArray();
        var value = 10;
        var splitFormat = path.Split(".").ToArray();
        var res = JObjectHelper.AddToJsonByPath(data, splitFormat, value);
        Assert.Equal(10, res[10]);
    }
    
    [Theory]
    [InlineData("[1].[1]")]
    public void Test6(string path)
    {
        var data = new JArray();
        var value = 10;
        var splitFormat = path.Split(".").ToArray();
        var res = JObjectHelper.AddToJsonByPath(data, splitFormat, value);
        Assert.Equal(10, res[1][1]);
    }
    
    [Theory]
    [InlineData("key.key.key")]
    public void Test7(string path)
    {
        var data = new JObject()
        {
            {"key", new JObject()},
            {"ke2", 11},
            {"key3", new JObject()
            {
                {"key4", 12}
            }}
        };

        var splitFormat = path.Split(".").ToArray();
        var res = JObjectHelper.AddToJsonByPath(data, splitFormat, 10);
        Assert.Equal(10, res["key"]["key"]["key"]);
        Assert.Equal(11, res["ke2"]);
        Assert.Equal(12, res["key3"]["key4"]);
    }
    
    [Theory]
    [InlineData("[1].name")]
    public void Test8(string path)
    {
        var data = new JArray();
        var value = 10;
        var splitFormat = path.Split(".").ToArray();
        var res = JObjectHelper.AddToJsonByPath(data, splitFormat, value);
        Assert.Equal(10, res[1]["name"]);
    }
    
    [Theory]
    [InlineData("name.[1].d")]
    public void Test9(string path)
    {
        var data = new JObject();
        var value = 10;
        var splitFormat = path.Split(".").ToArray();
        var res = JObjectHelper.AddToJsonByPath(data, splitFormat, value);
        Assert.Equal(10, res["name"][1]["d"]);
    }
    
    [Theory]
    [InlineData("name.[1].d.ggg.dfg")]
    public void Test10(string path)
    {
        var data = new JObject();
        var value = 10;
        var splitFormat = path.Split(".").ToArray();
        var res = JObjectHelper.AddToJsonByPath(data, splitFormat, value);
        Assert.Equal(10, res["name"][1]["d"]["ggg"]["dfg"]);
    }
    
    [Theory]
    [InlineData("name.[1].d")]
    public void Test11(string path)
    {
        var data = new JObject();
        var value = 10;
        var splitFormat = path.Split(".").ToArray();
        var res = JObjectHelper.AddToJsonByPath(data, splitFormat, value);
        Assert.Equal(10, res["name"][1]["d"]);
    }
    
    [Theory]
    [InlineData("name.[1].d.[3]")]
    public void Test12(string path)
    {
        var data = new JObject();
        var value = 10;
        var splitFormat = path.Split(".").ToArray();
        var res = JObjectHelper.AddToJsonByPath(data, splitFormat, value);
        Assert.Equal(10, res["name"][1]["d"][3]);
    }
    
    [Theory]
    [InlineData("name.[1].d.ggg.dfg")]
    public void Test13(string path)
    {
        var data = new JObject()
        {
            {"name", new JArray()
            {
                new JObject()
                {
                    { "test", "text" }
                }
            }}
        };
        var value = 10;
        var splitFormat = path.Split(".").ToArray();
        var res = JObjectHelper.AddToJsonByPath(data, splitFormat, value);
        Assert.Equal(10, res["name"][1]["d"]["ggg"]["dfg"]);
        Assert.Equal("text", res["name"][0]["test"]);
    }
    
    [Fact]
    public void Test15()
    {
        var data = new JObject();

        var res = JObjectHelper.AddToJsonByPath(data, new []{ "name" }, 10);
        var res2 = JObjectHelper.AddToJsonByPath(res, new []{ "data" }, new JObject()
        {
            {"test", "test"}
        });
        var res3 = JObjectHelper.AddToJsonByPath(res2, new []{ "data", "inner" }, true);
        var res4 = JObjectHelper.AddToJsonByPath(res3, new []{ "array", "[1]" }, 1);
        var res5 = JObjectHelper.AddToJsonByPath(res4, new []{ "array", "[0]" }, 2);
        
        
        Assert.Equal("test", res5["data"]["test"]);
        Assert.Equal(10, res5["name"]);
        Assert.Equal(true, res5["data"]["inner"]);
        Assert.Equal(2, res5["array"][0]);
        Assert.Equal(1, res5["array"][1]);
    }
}