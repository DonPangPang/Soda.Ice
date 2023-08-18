using Soda.Ice.Abstracts;

namespace Soda.Ice.Domain;

public class BlogViewLog : EntityBase, ICreator
{
    public Guid BlogId { get; set; }
    public Blog? Blog { get; set; }
    public Guid CreatorId { get; set; }
    public User? Creator { get; set; }
    public DateTime CreateTime { get; set; }
}