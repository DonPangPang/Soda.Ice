using Soda.Ice.Abstracts;

namespace Soda.Ice.Domain;

public class LoginLog : EntityBase, ICreator
{
    public string IpAddress { get; set; } = string.Empty;
    public string UA { get; set; } = string.Empty;

    public Guid CreatorId { get; set; }
    public User? Creator { get; set; }
    public DateTime CreateTime { get; set; }
}
