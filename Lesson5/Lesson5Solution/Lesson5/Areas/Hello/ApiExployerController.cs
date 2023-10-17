using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Lesson5.Areas.Hello;

[Route("api")]
public class ApiExployerController : Controller
{
    private readonly IApiDescriptionGroupCollectionProvider _apiExplorer;
    
    public ApiExployerController(IApiDescriptionGroupCollectionProvider apiExplorer)
    {
        _apiExplorer = apiExplorer;
    }
    
    [HttpPost]
    public IActionResult Index()
    {
        var response = _apiExplorer.ApiDescriptionGroups.Items.Select(value =>
        {
            return new
            {
                value.GroupName,
                Items = value.Items.Select(apiDescription =>
                {
                    return new
                    {
                        apiDescription.GroupName,
                        apiDescription.HttpMethod,
                        apiDescription.RelativePath,
                        apiDescription.Properties,
                        apiDescription.ParameterDescriptions,
                        apiDescription.SupportedResponseTypes
                    };
                })
            };
        }).ToList();
        return Ok(response);
    }
}