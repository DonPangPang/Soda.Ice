using Soda.Ice.Abstracts;
using System.Xml.Linq;

namespace Soda.Ice.Shared.ViewModels;

public record VBlog : ViewModel, ICreator, IModifior, ISoftDeleted
{
    public string Title { get; set; } = string.Empty;

    public ICollection<VBlogGroup> BlogGroups { get; set; } = new HashSet<VBlogGroup>();
    public ICollection<VBlogTag> BlogTags { get; set; } = new HashSet<VBlogTag>();

    public string Description { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public Guid? TitleImageId { get; set; }
    public VFileResource? TitleImage { get; set; }

    public Guid? LocalBlogFileId { get; set; }
    public VFileResource? LocalBlogFile { get; set; }

    public ICollection<VFileResource> FileResources { get; set; } = new List<VFileResource>();

    public ICollection<VBlogViewLog> BlogViewLogs { get; set; } = new List<VBlogViewLog>();

    public ICollection<VComment> Comments { get; set; } = new List<VComment>();

    public int BlogViewLogCount { get; set; }
    public int CommentsLength { get; set; }

    public Guid CreatorId { get; set; }
    public VUser? Creator { get; set; }
    public DateTime CreateTime { get; set; }
    public Guid? ModifiorId { get; set; }
    public VUser? Modifior { get; set; }
    public DateTime? UpdateTime { get; set; }
    public bool Deleted { get; set; } = false;
}

public record VBlogTiny : ViewModel
{
    public string Title { get; set; } = string.Empty;

    public ICollection<VBlogGroup> BlogGroups { get; set; } = new HashSet<VBlogGroup>();
    public ICollection<VBlogTag> BlogTags { get; set; } = new HashSet<VBlogTag>();

    public string Description { get; set; } = string.Empty;
    public Guid? TitleImageId { get; set; }
    public VFileResource? TitleImage { get; set; }

    public DateTime CreateTime { get; set; }
}