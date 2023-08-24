using Soda.Ice.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Soda.Ice.Domain;

public class BlogTag : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    [NotMapped]
    public int BlogCount => Blogs.Count;
}