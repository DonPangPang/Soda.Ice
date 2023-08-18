using Soda.Ice.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Soda.Ice.Domain;

public class Blog : EntityBase, ICreator, IModifior
{
    public string Title { get; set; } = string.Empty;

    public ICollection<BlogGroup> BlogGroups { get; set; } = new HashSet<BlogGroup>();
    public ICollection<BlogTag> BlogTags { get; set; } = new HashSet<BlogTag>();

    [NotMapped]
    public string Descrption => Content[..(Content.Length > 200 ? 200 : Content.Length)];

    public string Content { get; set; } = string.Empty;

    public ICollection<BlogViewLog> BlogViewLogs { get; set; } = new List<BlogViewLog>();

    [NotMapped]
    public int BlogViewLogCount => BlogViewLogs.Count;

    public Guid CreatorId { get; set; }
    public User? Creator { get; set; }
    public DateTime CreateTime { get; set; }
    public Guid? ModifiorId { get; set; }
    public User? Modifior { get; set; }
    public DateTime? UpdateTime { get; set; }
}
