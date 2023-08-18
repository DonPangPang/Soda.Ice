using Soda.Ice.Abstracts;

namespace Soda.Ice.Shared.ViewModels;

public record VLoginLog : ViewModel, ICreator
{
    public string IpAddress { get; set; } = string.Empty;
    public string UA { get; set; } = string.Empty;

    public Guid CreatorId { get; set; }
    public VUser? Creator { get; set; }
    public DateTime CreateTime { get; set; }
}