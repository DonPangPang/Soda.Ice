using Microsoft.EntityFrameworkCore;
using Soda.Ice.Abstracts;
using Soda.Ice.Common.Helpers;
using Soda.Ice.Domain;
using Soda.Ice.WebApi.Services.CurrentUserServices;
using System.Reflection;

namespace Soda.Ice.WebApi.Data;

public class IceDbContext : DbContext
{
    private readonly Lazy<Session> _session;

    public IceDbContext(DbContextOptions<IceDbContext> options, Lazy<Session> session) : base(options)
    {
        _session = session;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityTypes = TypeHelper.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            if (modelBuilder.Model.FindEntityType(entityType) is null)
            {
                modelBuilder.Model.AddEntityType(entityType);
            }
        }

        modelBuilder.Entity<User>().HasData(new User
        {
            Name = "Admin",
            IsSuper = true,
            Account = "admin",
            Password = "123456",
            Enabled = true,
        });

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    public override int SaveChanges()
    {
        try
        {
            AutoSetChangedEntities();
            return base.SaveChanges();
        }
        catch (DbUpdateException)
        {
            throw;
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            AutoSetChangedEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw;
        }
    }

    private void AutoSetChangedEntities()
    {
        foreach (var dbEntityEntry in ChangeTracker.Entries<EntityBase>())
        {
            var baseEntity = dbEntityEntry.Entity;
            switch (dbEntityEntry.State)
            {
                case EntityState.Added:
                    if (baseEntity is ICreator creatorEntity)
                    {
                        creatorEntity.CreateTime = DateTime.Now;
                        creatorEntity.CreatorId = _session.Value.UserId;
                    }
                    break;

                case EntityState.Modified:
                    if (baseEntity is IModifior modifiedEntity)
                    {
                        modifiedEntity.UpdateTime = DateTime.Now;
                        modifiedEntity.ModifiorId = _session.Value.UserId;
                    }
                    break;

                case EntityState.Deleted:
                    if (baseEntity is ISoftDeleted deletedEntity)
                    {
                        deletedEntity.Deleted = true;
                        dbEntityEntry.State = EntityState.Modified;
                    }
                    break;
            }
        }
    }
}