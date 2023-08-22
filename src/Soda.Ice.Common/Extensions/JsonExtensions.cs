using Newtonsoft.Json;
using Soda.Ice.Abstracts;

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

    /// <summary>
    /// 转为树形结构
    /// </summary>
    /// <param name="data"> </param>
    /// <typeparam name="T"> </typeparam>
    /// <returns> </returns>
    public static IEnumerable<T> ToTree<T>(this IEnumerable<T> data) where T : ITree<T>
    {
        var tree = new List<T>();
        foreach (var item in data)
        {
            if (item.ParentId == null || item.ParentId == Guid.Empty)
            {
                tree.Add(item);
            }

            foreach (var it in data)
            {
                if (it.ParentId == item.Id)
                {
                    if (item.Children == null)
                    {
                        item.Children = new List<T>();
                    }
                    item.Children.Add(it);
                }
            }
        }
        return tree;
    }
}