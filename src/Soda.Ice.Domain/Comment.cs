using Soda.Ice.Abstracts;

namespace Soda.Ice.Domain;

public class Comment : EntityBase, ICreator, IEnabled, ISoftDeleted, ITree<Comment>
{
    public string? Content { get; set; }

    public Guid CreatorId { get; set; }
    public User? Creator { get; set; }

    public Guid? ReplyorId { get; set; }
    public User? Replyor { get; set; }

    public DateTime CreateTime { get; set; }
    public bool HasRead { get; set; } = false;
    public bool Enabled { get; set; }
    public bool Deleted { get; set; }
    public Guid? ParentId { get; set; }
    public ICollection<Comment>? Children { get; set; }
    public string? TreeIds { get; set; }
}