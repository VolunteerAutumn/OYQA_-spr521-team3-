using MoviesCatalog.BLL.DTOs;

namespace MoviesCatalog.BLL.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto model);
    Task<string> LoginAsync(LoginDto model);
    Task<bool> ConfirmEmailAsync(string userId, string token);
    Task ForgotPasswordAsync(string email);
    Task ResetPasswordAsync(ResetPasswordDto model);
    Task ChangePasswordAsync(ChangePasswordDto model);
    Task AssignRoleAsync(AssignRoleDto model);
}