using Newtonsoft.Json;

namespace Soda.Ice.Common.Extensions;

public static class JsonExtensions
{
    public static string ToJson<T>(this T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static T? ToObject<T>(this string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}