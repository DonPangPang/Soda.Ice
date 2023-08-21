using Soda.Ice.Abstracts;

namespace Soda.Ice.Shared.ViewModels;

public record VBlog : ViewModel, ICreator, IModifior, ISoftDeleted
{
    public string Title { get; set; } = string.Empty;

    public ICollection<VBlogGroup> BlogGroups { get; set; } = new HashSet<VBlogGroup>();
    public ICollection<VBlogTag> BlogTags { get; set; } = new HashSet<VBlogTag>();

    public string Descrption => Content[..(Content.Length > 200 ? 200 : Content.Length)];

    public string Content { get; set; } = string.Empty;
    public ICollection<VFileResource> FileResources { get; set; } = new List<VFileResource>();

    public ICollection<VBlogViewLog> BlogViewLogs { get; set; } = new List<VBlogViewLog>();

    public int BlogViewLogCount => BlogViewLogs.Count;

    public Guid CreatorId { get; set; }
    public VUser? Creator { get; set; }
    public DateTime CreateTime { get; set; }
    public Guid? ModifiorId { get; set; }
    public VUser? Modifior { get; set; }
    public DateTime? UpdateTime { get; set; }
    public bool Deleted { get; set; } = false;
}