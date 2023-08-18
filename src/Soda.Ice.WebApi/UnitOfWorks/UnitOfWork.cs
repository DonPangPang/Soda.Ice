using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Soda.Ice.Abstracts;
using Soda.Ice.WebApi.Data;
using Soda.Ice.WebApi.Extensions;
using Soda.Ice.WebApi.Services.CurrentUserServices;

namespace Soda.Ice.WebApi.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly IceDbContext _dbContext;
    private readonly Lazy<Session> _session;

    public UnitOfWork(IceDbContext dbContext, Lazy<Session> session)
    {
        _dbContext = dbContext;
        _session = session;
    }

    public async Task<bool> CommitAsync()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public IQueryable<T> Query<T>() where T : class, IEntity
    {
        if (_session.Value.User?.IsSuper ?? false)
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        return _dbContext.Set<T>().AsQueryable().FilterCurrentUser(_session.Value.UserId);
    }

    public DbSet<T> Table<T>() where T : class, IEntity
    {
        return _dbContext.Set<T>();
    }

    public DbContext Db => _dbContext;
}