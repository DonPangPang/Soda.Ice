using Soda.Ice.Abstracts;
using Soda.Ice.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Soda.Ice.Domain;

public class User : EntityBase, IEnabled
{
    public string Name { get; set; } = string.Empty;
    public Guid AvatorId { get; set; }

    [NotMapped]
    public FileResource? Avator { get; set; }

    public Gender Gender { get; set; }
    public string? Email { get; set; }
    public string? Descrption { get; set; }
    public string Account { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public bool IsSuper { get; set; } = false;
}