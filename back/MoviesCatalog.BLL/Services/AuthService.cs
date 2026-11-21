using Microsoft.AspNetCore.Identity;
using MoviesCatalog.BLL.DTOs;
using MoviesCatalog.BLL.Interfaces;
using MoviesCatalog.DAL.Entities;
using System.Web;

namespace MoviesCatalog.BLL.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ITokenService tokenService,
        IEmailService emailService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _emailService = emailService;
    }

    public async Task<string> RegisterAsync(RegisterDto model)
    {
        DateTime? safeDateOfBirth = model.DateOfBirth.HasValue
            ? DateTime.SpecifyKind(model.DateOfBirth.Value, DateTimeKind.Unspecified)
            : null;

        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            DateOfBirth = safeDateOfBirth,
            IsSubscribedToNewsletter = model.IsSubscribedToNewsletter
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }
        else
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception($"User creation failed: {errors}");
        }

        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(emailToken);
        var confirmationLink = $"http://localhost:5000/api/Auth/confirm-email?userId={user.Id}&token={encodedToken}";

        await _emailService.SendEmailAsync(user.Email!, "Confirm your email", $"Please confirm your email by clicking this link: {confirmationLink}");

        // GET ROLES AND PASS TO TOKEN
        var roles = await _userManager.GetRolesAsync(user);
        return _tokenService.CreateToken(user, roles);
    }

    public async Task<string> LoginAsync(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) throw new Exception("Invalid email or password.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!isPasswordValid) throw new Exception("Invalid email or password.");

        // GET ROLES AND PASS TO TOKEN
        var roles = await _userManager.GetRolesAsync(user);
        return _tokenService.CreateToken(user, roles);
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new Exception("User not found.");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return;

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(resetToken);
        var resetLink = $"http://localhost:5000/api/Auth/reset-password?email={user.Email}&token={encodedToken}";

        await _emailService.SendEmailAsync(user.Email!, "Reset Password", $"Please reset your password by clicking this link: {resetLink}");
    }

    public async Task ResetPasswordAsync(ResetPasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) throw new Exception("User not found.");

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception($"Password reset failed: {errors}");
        }
    }

    public async Task ChangePasswordAsync(ChangePasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) throw new Exception("User not found.");

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception($"Password change failed: {errors}");
        }
    }

    public async Task AssignRoleAsync(AssignRoleDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) throw new Exception("User not found.");

        if (!await _roleManager.RoleExistsAsync(model.Role))
        {
            await _roleManager.CreateAsync(new IdentityRole(model.Role));
        }

        var result = await _userManager.AddToRoleAsync(user, model.Role);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception($"Failed to assign role: {errors}");
        }
    }
}