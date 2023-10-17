using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Lesson5.Controllers;

public  class MaxValueAttribute : ValidationAttribute
{
    private readonly int _maxValue;

    public MaxValueAttribute(int maxValue)
    {
        _maxValue = maxValue;
    }

    public override bool IsValid(object value)
    {
        return (int) value <= _maxValue;
    }
}


public class ValidationDto : IValidatableObject
{
    public string Name { get; init; }
    
    public int Year { get; init; }
    
    public int? Age { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationList = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(Name))
        {
            validationList.Add(
                new ValidationResult(
                    $"Неверный формат {nameof(Name)}: {Name}",
                    new[] {nameof(Name)}
                ));
        }

        return validationList;
    }
}

public class AttributeValidationDto
{
    [MinLength(10)]
    [Required]
    public string Name { get; init; }
    
    [MaxValue(2000)]
    public int Year { get; init; }
    
    [Required]
    [Range(1, 50)]
    public int? Age { get; init; }
}

[ApiController]
[Route("test1")]
public abstract class ValidationController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateUserAsync(CreateUserRequest dto)
    {
        return Ok(dto);
    }
}