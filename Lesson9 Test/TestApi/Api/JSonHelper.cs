using Newtonsoft.Json.Linq;

namespace Api;

public static class JObjectHelper
{
    /// <summary>
    /// Получить только цифры
    /// </summary>
    public static string GetOnlyDigits(this string str)
    {
        var result = new string(str.Where(char.IsDigit).ToArray());
        return result;
    }

    /// <summary>
    /// Запись в json по массиву пути
    /// принимает подобные конструкции new {"name", "[1]", "d", "[3]"}
    /// </summary>
    public static JToken AddToJsonByPath(JToken jObject, string[] keyPath, JToken value)
    {
        if (keyPath.Length == 0)
        {
            return jObject;
        }

        if (keyPath.Length == 1)
        {
            if (keyPath[0].Contains("[") && keyPath[0].Contains("]"))
            {
                if (jObject == null || jObject is JValue jValue && jValue.Value == null)
                {
                    jObject = new JArray();
                }

                if (jObject is JObject)
                {
                    jObject = new JArray();
                }

                SetJArray((JArray)jObject, keyPath[0], value);
                return jObject;
            }

            if (jObject is JValue)
            {
                return new JObject()
                {
                    { keyPath[0], value }
                };
            }

            jObject[keyPath[0]] = value;
            return jObject;
        }

        JToken inner;
        try
        {
            if (jObject is JValue && !keyPath[0].Contains("[") && !keyPath[0].Contains("]"))
            {
                jObject = new JObject();
            }

            inner = jObject[keyPath[0]];
        }
        catch (ArgumentException)
        {
            try
            {
                inner = jObject[Convert.ToInt32(keyPath[0].GetOnlyDigits())];
            }
            catch (ArgumentOutOfRangeException)
            {
                if (jObject is JArray)
                {
                    inner = jObject;
                }
                else
                {
                    inner = new JArray();
                }
            }
        }

        if (keyPath[0].Contains("[") && keyPath[0].Contains("]"))
        {
            var array = (JArray)inner ?? new JArray();
            var index = Convert.ToInt32(keyPath[0].GetOnlyDigits());
            SetCountJArray(index, array);

            jObject = array;
            inner = jObject[index];
        }

        if (inner == null)
        {
            jObject[keyPath[0]] = new JObject();
            inner = jObject[keyPath[0]];
        }

        var newKeyList = keyPath.Skip(1).ToArray();
        var res = AddToJsonByPath(inner, newKeyList, value);
        if (jObject is JObject jObjectRes)
        {
            jObjectRes[keyPath[0]] = res;
            return jObjectRes;
        }

        if (jObject is JArray jArrayRes)
        {
            var index = Convert.ToInt32(keyPath[0].GetOnlyDigits());
            jArrayRes[index] = res;
            return jArrayRes;
        }

        throw new Exception();
    }

    private static void SetJArray(JToken jObject, string key, JToken value)
    {
        var index = Convert.ToInt32(key.GetOnlyDigits());
        var array = (JArray)jObject;
        SetCountJArray(index, array);

        array[index] = value;
    }

    private static void SetCountJArray(int index, JArray array)
    {
        for (var i = 0; i <= index; i++)
        {
            try
            {
                array[i] = array[i];
            }
            catch (ArgumentOutOfRangeException)
            {
                array.Insert(i, null);
            }
        }
    }
}