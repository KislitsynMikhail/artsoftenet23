using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.Cryptography;
using Api.Filters;
using Extender;
using Lesson1;
using Lesson1.Attributes;
using Lesson1.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Filters

builder.Services.AddTransient<IService, FirstService>();
builder.Services.FiltersLesson();

#endregion

var app = builder.Build();

#region Attribute

////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#region Decorator example

//DecoratorVsAttribute.Example();

#endregion

#region example meta attribute

app.UseMiddleware<MyMiddleware>();

MyClass myClassInstance = new MyClass { ScreenName = null };

//Проверка атрибутов у свойств класса
var propertiesList = myClassInstance.GetType().GetProperties();
foreach (var property in propertiesList)
{
    var instanceCustomAttributes = property.GetCustomAttribute(typeof(MyRequiredAttribute)) as MyRequiredAttribute;
    var qwerty = property.GetCustomAttribute<MyRequiredAttribute>();
}

//Получение атрибутов для класса
var atrributeByMyClass = myClassInstance.GetType().GetCustomAttributesData();

#endregion


#region RuntimeAttr


var typeExtender = new TypeExtender("MyClass");

var returnedType = typeExtender.FetchType();

var atrributeByMyClass1 = returnedType.GetCustomAttributesData();

typeExtender.AddAttribute<RuntimeAddingAttr>(new []{"Исходные данные"});

atrributeByMyClass1 = returnedType.GetCustomAttributesData();

#endregion

////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();

public class RuntimeAddingAttr : Attribute
{
    public RuntimeAddingAttr(string dd)
    {
    }
}