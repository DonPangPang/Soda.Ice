using Soda.Ice.Abstracts;

namespace Soda.Ice.Domain;

public class FileResource : EntityBase, ICreator
{
    public string Name { get; set; } = string.Empty;
    public string Suffix { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;

    public Guid? BlogId { get; set; }
    public Blog? Blog { get; set; }

    public Guid CreatorId { get; set; }
    public User? Creator { get; set; }
    public DateTime CreateTime { get; set; }
}