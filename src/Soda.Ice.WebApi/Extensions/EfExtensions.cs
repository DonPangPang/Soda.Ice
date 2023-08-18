using Soda.Ice.Abstracts;
using System.Linq.Expressions;

namespace Soda.Ice.WebApi.Extensions;

public static class EfExtensions
{
    public static IQueryable<T> FilterCurrentUser<T>(this IQueryable<T> query, Guid userId)
    {
        if (query is IQueryable<ICreator> temp)
        {
            temp.Where(x => x.CreatorId == userId);

            return (temp as IQueryable<T>)!;
        }

        return query;
    }
}