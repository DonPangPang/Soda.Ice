using Soda.Ice.Abstracts;
using System.Xml.Linq;

namespace Soda.Ice.Shared.ViewModels;

public record VBlog : ViewModel, ICreator, IModifior, ISoftDeleted
{
    public string Title { get; set; } = string.Empty;

    public ICollection<VBlogGroup> BlogGroups { get; set; } = new HashSet<VBlogGroup>();
    public ICollection<VBlogTag> BlogTags { get; set; } = new HashSet<VBlogTag>();

    public string Descrption => Content[..(Content.Length > 105 ? 105 : Content.Length)];

    public string Content { get; set; } = string.Empty;

    public Guid? LocalBlogFileResourceId { get; set; }
    public VFileResource? LocalBlogFileResource { get; set; }

    public ICollection<VFileResource> FileResources { get; set; } = new List<VFileResource>();

    public ICollection<VBlogViewLog> BlogViewLogs { get; set; } = new List<VBlogViewLog>();

    public ICollection<VComment> Comments { get; set; } = new List<VComment>();

    public int BlogViewLogCount => BlogViewLogs.Count;
    public int CommentsLength => Comments.Count;

    public Guid CreatorId { get; set; }
    public VUser? Creator { get; set; }
    public DateTime CreateTime { get; set; }
    public Guid? ModifiorId { get; set; }
    public VUser? Modifior { get; set; }
    public DateTime? UpdateTime { get; set; }
    public bool Deleted { get; set; } = false;
}

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