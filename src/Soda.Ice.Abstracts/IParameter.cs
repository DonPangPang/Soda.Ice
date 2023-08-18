using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Soda.Ice.Abstracts;

public interface IParameters
{
    IEnumerable<PropertyInfo> GetProperties();
}

public class IceParameters : IParameters
{
    public IEnumerable<PropertyInfo> GetProperties()
    {
        var properties = GetType().GetProperties().Where(p => !typeof(IPaging).GetProperties().Select(x => x.Name).Contains(p.Name) &&
                            !typeof(ISorting).GetProperties().Select(x => x.Name).Contains(p.Name) &&
                            !typeof(IDateRange).GetProperties().Select(x => x.Name).Contains(p.Name) && p.GetValue(this) != null);

        return properties;
    }
}

public interface IPaging
{
    /// <summary>
    /// 页码
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// 每页大小
    /// </summary>
    public int PageSize { get; set; }
}

public interface ISorting
{
    /// <summary>
    /// 排序
    /// </summary>
    public string? OrderBy { get; set; }
}

public interface IDateRange
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}