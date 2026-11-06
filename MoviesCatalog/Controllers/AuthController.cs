using Microsoft.AspNetCore.Mvc;
using MoviesCatalog.BLL.DTOs;
using MoviesCatalog.BLL.Interfaces;

namespace MoviesCatalog.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        try
        {
            var token = await _authService.RegisterAsync(model);
            return Ok(new { Token = token, message = "Registration successful. Check your email for confirmation token." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        try
        {
            var token = await _authService.LoginAsync(model);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        try
        {
            var success = await _authService.ConfirmEmailAsync(userId, token);
            if (success) return Ok(new { message = "Email confirmed successfully." });
            return BadRequest(new { message = "Email confirmation failed." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
    {
        try
        {
            await _authService.ForgotPasswordAsync(model.Email);
            return Ok(new { message = "If the email exists, a password reset token has been sent." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
    {
        try
        {
            await _authService.ResetPasswordAsync(model);
            return Ok(new { message = "Password has been reset successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
    {
        try
        {
            await _authService.ChangePasswordAsync(model);
            return Ok(new { message = "Password changed successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto model)
    {
        try
        {
            await _authService.AssignRoleAsync(model);
            return Ok(new { message = $"Role '{model.Role}' assigned successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}