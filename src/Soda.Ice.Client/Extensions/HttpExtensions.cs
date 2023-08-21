using Soda.Ice.Abstracts;
using System.Text;
using System;
using System.Collections;

namespace Soda.Ice.Client.Extensions;

public static class HttpExtensions
{
    /// <summary>
    /// 拼接参数
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="parameter"> </param>
    /// <returns> </returns>
    public static string GetQueryString<T>(this T parameter) where T : IParameters
    {
        var sb = new StringBuilder();
        foreach (var kvp in parameter.GetType().GetProperties())
        {
            var key = kvp.Name;
            var value = kvp.GetValue(parameter);

            if (value is Array array)
            {
                foreach (var item in array)
                {
                    sb.Append($"&{key}={item}");
                }
            }
            else if (value is IList)
            {
                foreach (var item in (IList)value)
                {
                    sb.Append($"&{key}={item}");
                }
            }
            else
            {
                sb.Append($"&{key}={value}");
            }
        }

        return $"?{sb.ToString().TrimStart('&')}";

        //var props = parameter.GetType().GetProperties();

        //var querys = new List<string>();

        //foreach (var prop in props)
        //{
        //    querys.Add($"{prop.Name}={prop.GetValue(parameter)}");
        //}

        //return "?" + string.Join("&", querys);
    }
}