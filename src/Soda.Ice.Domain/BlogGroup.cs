using Soda.Ice.Abstracts;

namespace Soda.Ice.Domain;

public class BlogGroup : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}