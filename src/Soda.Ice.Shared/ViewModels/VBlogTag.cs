using Soda.Ice.Abstracts;

namespace Soda.Ice.Shared.ViewModels;

public record VBlogTag : ViewModel
{
    public string Name { get; set; } = string.Empty;
    public ICollection<VBlog> Blogs { get; set; } = new List<VBlog>();
}