using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(o =>
    {
        o.ValueProviderFactories.RemoveType<QueryStringValueProviderFactory>();
        o.ValueProviderFactories.Add(new SeparatedQueryStringValueProviderFactory(","));
        o.ValueProviderFactories.Add(new CookieValueProviderFactory());

        o.ModelBinderProviders.Insert(0,new PhoneModelBinderProvider());
    })
    .AddNewtonsoftJson(option =>
    {
       option.SerializerSettings.Converters.Add(new PhoneConverter());
    }).AddXmlSerializerFormatters();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();

public class PhoneConverter : JsonConverter<Phone>
{
    /// <inheritdoc />
    public override Phone ReadJson(JsonReader reader, Type objectType, Phone existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value == null)
        {
            return new Phone();
        }

        if (reader.Value is string stringValue)
        {
            return new Phone(stringValue);
        }

        throw new Exception();
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, Phone value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }
}

public class CookieValueProviderFactory : IValueProviderFactory
{
    public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));

        var cookies = context.ActionContext.HttpContext.Request.Cookies;
        if (cookies.Count > 0)
        {
            var valueProvider = new CookieValueProvider(
                BindingSource.ModelBinding,
                cookies,
                CultureInfo.InvariantCulture);

            context.ValueProviders.Add(valueProvider);
        }

        return Task.CompletedTask;
    }
}

public class CookieValueProvider : BindingSourceValueProvider, IEnumerableValueProvider
{
    private readonly IRequestCookieCollection _values;
    private PrefixContainer? _prefixContainer;

    public CookieValueProvider(BindingSource bindingSource, IRequestCookieCollection values, CultureInfo culture) : base(bindingSource)
    {
        _ = bindingSource ?? throw new ArgumentNullException(nameof(bindingSource));
        _ = values ?? throw new ArgumentNullException(nameof(values));

        (_values, Culture) = (values, culture);
    }

    public CultureInfo Culture { get; }

    protected PrefixContainer PrefixContainer =>
        _prefixContainer ??= new PrefixContainer(_values.Keys);

    public override bool ContainsPrefix(string prefix) =>
        PrefixContainer.ContainsPrefix(prefix);

    public virtual IDictionary<string, string> GetKeysFromPrefix(string prefix)
    {
        _ = prefix ?? throw new ArgumentNullException(nameof(prefix));

        return PrefixContainer.GetKeysFromPrefix(prefix);
    }


    public override ValueProviderResult GetValue(string key)
    {
        _ = key ?? throw new ArgumentNullException(nameof(key));

        if (key.Length == 0)
        {
            return ValueProviderResult.None;
        }

        var value = _values[key];
        if (string.IsNullOrEmpty(value))
        {
            return ValueProviderResult.None;
        }

        return new ValueProviderResult(value, Culture);
    }
}

/// <summary>
/// An <see cref="IModelBinderProvider"/> for binding <see cref="DateTime" /> and nullable <see cref="DateTime"/> models.
/// </summary>
public class PhoneModelBinderProvider : IModelBinderProvider
{
    /// <inheritdoc />
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var modelType = context.Metadata.UnderlyingOrModelType;
        if (modelType == typeof(Phone))
        {
            // я не стал делать логи, но хотел показать, что тут можно из di извлекать данные
            var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
            return new PhoneModelBinder();
        }

        return null;
    }
}

public struct Phone
{
    public string Value { get; init; }

    public bool IsNullOrEmpty() => string.IsNullOrWhiteSpace(Value);

    public Phone()
    {
        Value = "";
    }
    
    public Phone(string value)
    {
        Value = value;
    }
}

/// <summary>
/// An <see cref="IModelBinder"/> for <see cref="DateTime"/> and nullable <see cref="DateTime"/> models.
/// </summary>
public class PhoneModelBinder : IModelBinder
{
    /// <inheritdoc />
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }
        
        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        var modelState = bindingContext.ModelState;
        modelState.SetModelValue(modelName, valueProviderResult);

        var metadata = bindingContext.ModelMetadata;
        var type = metadata.UnderlyingOrModelType;
        try
        {
            var value = valueProviderResult.FirstValue;

            Phone model;
            if (string.IsNullOrWhiteSpace(value))
            {
                // Parse() method trims the value (with common DateTimeSyles) then throws if the result is empty.
                model = new Phone();
            }
            else if (type == typeof(Phone))
            {
                model = new Phone(value);
            }
            else
            {
                throw new NotSupportedException();
            }

            // When converting value, a null model may indicate a failed conversion for an otherwise required
            // model (can't set a ValueType to null). This detects if a null model value is acceptable given the
            // current bindingContext. If not, an error is logged.
            if (model.IsNullOrEmpty() && !metadata.IsReferenceOrNullableType)
            {
                modelState.TryAddModelError(
                    modelName,
                    metadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(
                        valueProviderResult.ToString()));
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(model);
            }
        }
        catch (Exception exception)
        {
            // Conversion failed.
            modelState.TryAddModelError(modelName, exception, metadata);
        }
        
        return Task.CompletedTask;
    }
}

public class SeparatedQueryStringValueProviderFactory : IValueProviderFactory
{
    private readonly string _separator;

    public SeparatedQueryStringValueProviderFactory(string separator)
    {
        _separator = separator;
    }

    public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var query = context.ActionContext.HttpContext.Request.Query;
        if (query.Count > 0)
        {
            context.ValueProviders
                .Add(new SeparatedQueryStringValueProvider(query, _separator));
        }

        return Task.CompletedTask;
    }
}

public class SeparatedQueryStringValueProvider : QueryStringValueProvider
{
    private readonly string _separator;

    public SeparatedQueryStringValueProvider(IQueryCollection values, string separator)
        : base(BindingSource.Query, values, CultureInfo.InvariantCulture)
    {
        _separator = separator;
    }

    public override ValueProviderResult GetValue(string key)
    {
        // получаем значени как раньше
        var result = base.GetValue(key);

        if (result == ValueProviderResult.None)
        {
            // немного валидации
            return result;
        }

        // смотрим есть ли установленный сепоратор
        var isExistSplit = result.Values.Any(x => x.IndexOf(_separator, StringComparison.OrdinalIgnoreCase) > 0);
        if (isExistSplit)
        {
            // сепорируем значения
            var splitValue = result.Values.SelectMany(x => x.Split(new[] { _separator }, StringSplitOptions.None))
                .ToArray();
            var splitValues = new StringValues(splitValue); // добавляем сепарированный вариат
            return new ValueProviderResult(splitValues, result.Culture);
        }

        return result;
    }
}