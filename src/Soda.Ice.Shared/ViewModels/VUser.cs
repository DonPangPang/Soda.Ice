using Soda.Ice.Abstracts;
using Soda.Ice.Shared.Enums;

namespace Soda.Ice.Shared.ViewModels;

public record VUser : ViewModel, IEnabled
{
    public string Name { get; set; } = string.Empty;
    public Guid AvatorId { get; set; }
    public VFileResource? Avator { get; set; }
    public Gender Gender { get; set; }
    public string? Email { get; set; }
    public string? Descrption { get; set; }
    public string Account { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}

public record VRegisterUser : VUser
{
    public string Password { get; set; } = string.Empty;
}