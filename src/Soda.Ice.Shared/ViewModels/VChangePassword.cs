namespace Soda.Ice.Shared.ViewModels;

public class VChangePassword
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;

    public bool Verify()
    {
        return NewPassword.Equals(ConfirmNewPassword);
    }
}