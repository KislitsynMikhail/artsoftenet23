using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Lesson1.Attributes;

/// <summary>
/// Пример класса, который валидирует атрибут '<inheritdoc cref="MyClassValidationAttribute"/>'>
/// </summary>
[MyClassValidation]
public class MyClass
{
    public string Inn { get; set; }
    
    public string Kpp { get; set; }
    
    [MyRequired]
    public string Name { get; set; }
    
    [NotNull]
    public string ScreenName
    {
        get => _screenName;
        set => _screenName = value ?? GenerateRandomScreenName();
    }

    private static string GenerateRandomScreenName()
    {
        return Guid.NewGuid().ToString();
    }

    private string _screenName;
}

/// <summary>
/// Переопределение сообщения коробочного атрибута валидации
/// </summary>
public class MyRequiredAttribute : RequiredAttribute
{
    /// <inheritdoc />
    public override string FormatErrorMessage(string name)
    {
        return $"My Required Attribute work: param by {name} value was empty";
    }
}

/// <summary>
/// Пример реализации валидационного атрибута
/// </summary>
public class MyClassValidationAttribute : ValidationAttribute
{
    /// <inheritdoc />
    public override bool IsValid(object value)
    {
        if (value is not MyClass myClass)
        {
            return true;
        }

        if (!string.IsNullOrWhiteSpace(myClass.Inn) && string.IsNullOrWhiteSpace(myClass.Kpp))
        {
            throw new Exception($"Объект {nameof(myClass.Kpp)} не был заполнен, хотя обязателе к заполнению, если передан телефон пользователя");
        }

        return true;
    }
}