namespace Soda.Ice.Abstracts;

public interface IEntity
{
    Guid Id { get; set; }
}

public abstract class EntityBase : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
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

/// <summary>
/// 树形数据
/// </summary>
public interface ITree<T>
{
    /// <summary>
    /// Id
    /// </summary>
    /// <value></value>
    public Guid Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public Guid? ParentId { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public ICollection<T>? Children { get; set; }

    /// <summary>
    /// 树状结构
    /// </summary>
    /// <value></value>
    public string? TreeIds { get; set; }
}