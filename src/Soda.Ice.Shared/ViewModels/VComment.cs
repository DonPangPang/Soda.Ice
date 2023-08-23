using Soda.Ice.Abstracts;

namespace Soda.Ice.Shared.ViewModels;

public record VComment : ViewModel, ICreator, ISoftDeleted, ITree<VComment>
{
    public string? Content { get; set; }

    public Guid CreatorId { get; set; }
    public VUser? Creator { get; set; }
    public DateTime CreateTime { get; set; }
    public bool Enabled { get; set; }
    public bool Deleted { get; set; }
    public Guid? ParentId { get; set; }
    public ICollection<VComment>? Children { get; set; }
    public string? TreeIds { get; set; }
}