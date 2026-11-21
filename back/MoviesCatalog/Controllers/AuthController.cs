using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailGet([FromQuery] string userId, [FromQuery] string token)
    {
        try
        {
            var success = await _authService.ConfirmEmailAsync(userId, token);
            if (success) return Ok(new { message = "Email confirmed successfully. You can close this tab." });
            return BadRequest(new { message = "Email confirmation failed." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
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

    [HttpGet("reset-password")]
    [AllowAnonymous]
    public IActionResult ResetPasswordGet([FromQuery] string email, [FromQuery] string token)
    {
        return Ok(new
        {
            message = "Token received successfully. Please use the POST /api/Auth/reset-password endpoint in Swagger to set your new password.",
            email = email,
            token = token
        });
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

    [Authorize(Roles = "Admin")]
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