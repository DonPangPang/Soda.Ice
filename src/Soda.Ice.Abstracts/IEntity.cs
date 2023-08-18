namespace Soda.Ice.Abstracts;

public interface IEntity
{
    Guid Id { get; set; }
}

public abstract class EntityBase : IEntity
{
    public Guid Id { get; set; }
}

public interface ISoftDeleted
{
    bool Deleted { get; set; }
}

public interface IEnabled
{
    bool Enabled { get; set; }
}

public interface ICreator
{
    public Guid CreatorId { get; set; }
    public DateTime CreateTime { get; set; }
}

public interface IModifior
{
    public Guid? ModifiorId { get; set; }
    public DateTime? UpdateTime { get; set; }
}