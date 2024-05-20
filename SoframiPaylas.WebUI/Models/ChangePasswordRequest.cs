namespace SoframiPaylas.WebUI.Models;
public class ChangePasswordRequest
{
    public string UserId { get; set; }
    public string NewPassword { get; set; }
    public string OldPassword { get; set; }
    public string Email { get; set; }
}