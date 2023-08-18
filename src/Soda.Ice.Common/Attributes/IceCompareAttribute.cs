using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soda.Ice.Common.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class IceCompareAttribute : Attribute
{
    public IceCompareAttribute(Operation comparer, string? propertyName = null)
    {
        Comparer = comparer;
        PropertyName = propertyName;
    }

    public Operation Comparer { get; }
    public string? PropertyName { get; }
}

public enum Operation
{
    Equal,
    Contains,
    GreaterThan,
    LessThan,
    GreaterThanOrEqual,
    LessThanOrEqual,
}