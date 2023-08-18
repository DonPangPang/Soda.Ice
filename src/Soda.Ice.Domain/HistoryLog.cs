using Soda.Ice.Abstracts;

namespace Soda.Ice.Domain;

public class HistoryLog : EntityBase, ICreator
{
    public Guid RelatrionId { get; set; }
    public string Content { get; set; } = string.Empty;

    public Guid CreatorId { get; set; }
    public User? Creator { get; set; }
    public DateTime CreateTime { get; set; }
}
