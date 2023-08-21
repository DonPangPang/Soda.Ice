using Soda.Ice.Abstracts;
using System.Reflection.Metadata;

namespace Soda.Ice.Shared.ViewModels;

public record VFileResource : ViewModel, ICreator
{
    public string Name { get; set; } = string.Empty;
    public string Suffix { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public Guid? BlogId { get; set; }
    public VBlog? Blog { get; set; }

    public Guid CreatorId { get; set; }
    public VUser? Creator { get; set; }
    public DateTime CreateTime { get; set; }
}