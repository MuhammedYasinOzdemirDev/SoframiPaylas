namespace SoframiPaylas.Application.DTOs.User;
public class ChangeUserPasswordDto
{
    public string UserId { get; set; }
    public string NewPassword { get; set; }
    public string OldPassword { get; set; }
    public string Email { get; set; }
}