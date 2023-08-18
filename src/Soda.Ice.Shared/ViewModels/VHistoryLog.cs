using Soda.Ice.Abstracts;

namespace Soda.Ice.Shared.ViewModels;

public record VHistoryLog : ViewModel, ICreator
{
    public Guid RelatrionId { get; set; }
    public string Content { get; set; } = string.Empty;

    public Guid CreatorId { get; set; }
    public VUser? Creator { get; set; }
    public DateTime CreateTime { get; set; }
}