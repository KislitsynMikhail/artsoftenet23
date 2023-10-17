using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Lesson5.Feature;

public class CustomControllerFeature : ControllerFeatureProvider 
{
    protected override bool IsController(TypeInfo typeInfo)
    {
        var isController = base.IsController(typeInfo);
        if (isController)
        {
            //Console.WriteLine($"{typeInfo.Name} IsController.");
        }
        
        return isController;
    }
}