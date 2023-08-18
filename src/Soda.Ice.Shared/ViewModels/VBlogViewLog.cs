using Soda.Ice.Abstracts;

namespace Soda.Ice.Shared.ViewModels;

public record VBlogViewLog : ViewModel, ICreator
{
    public Guid BlogId { get; set; }
    public VBlog? Blog { get; set; }
    public Guid CreatorId { get; set; }
    public VUser? Creator { get; set; }
    public DateTime CreateTime { get; set; }
}