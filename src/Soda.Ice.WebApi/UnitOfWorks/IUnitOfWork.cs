using Microsoft.EntityFrameworkCore;
using Soda.Ice.Abstracts;

namespace Soda.Ice.WebApi.UnitOfWorks;

public interface IUnitOfWork
{
    IQueryable<T> Query<T>() where T : class, IEntity;

    DbSet<T> Table<T>() where T : class, IEntity;

    Task<bool> CommitAsync();

    DbContext Db { get; }
}