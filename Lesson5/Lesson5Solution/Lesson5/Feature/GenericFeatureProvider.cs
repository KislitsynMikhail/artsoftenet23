using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lesson5.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Lesson5.Feature;

public class GenericFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();

        var allTestCaseV2 = currentAssembly
            .GetTypes()
            .Where(value => value.IsClass
                            && !value.IsAbstract
                            && value.IsSubclassOf(typeof(Test))
            )
            .ToList();

        foreach (var candidate in allTestCaseV2)
        {
            feature.Controllers.Add(typeof(GenericController<>).MakeGenericType(candidate).GetTypeInfo());
        }
    }
}

/// <summary>
/// Изменение динамическое пути
/// </summary>
public class GenericControllerRouteConvention : IControllerModelConvention
{
    /// <inheritdoc />
    public void Apply(ControllerModel controller)
    {
        if (controller.ControllerType.IsGenericType)
        {
            var genericType = controller.ControllerType.GenericTypeArguments[0];
            var routeString = $"test/{genericType.Name.ToLower()}/load";
            var route = new RouteAttribute(routeString);
            if (!string.IsNullOrWhiteSpace(routeString))
            {
                controller.Selectors
                    .Add(new SelectorModel
                    {
                        AttributeRouteModel = new AttributeRouteModel(route)
                    });
            }
        }
    }
}