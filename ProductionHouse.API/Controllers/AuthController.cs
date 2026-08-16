using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Core.Responses;
using System.Security.Claims;
namespace ProductionHouse.API.Controllers;

[AllowAnonymous]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        return Ok(new ApiResponse<LoginResponseDto>(
            true,
            "Login successful.",
            result
        ));
    }

    [Authorize]
    [HttpGet("Me")]
    public async Task<IActionResult> Me()
    {
        var user =
            await _authService.GetCurrentUserAsync(User);

        return Ok(new ApiResponse<CurrentUserDto>(
            true,
            "Current user retrieved successfully.",
            user));
    }
    [Authorize]
    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        return Ok(new ApiResponse<string>(
            true,
            "Logged out successfully."
        ));
    }
    [Authorize]
    [HttpPost("Change-Password")]
    public async Task<IActionResult> ChangePassword(
    ChangePasswordDto dto)
    {
        var id = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _authService.ChangePasswordAsync(id, dto);

        return Ok(new ApiResponse<string>(
            true,
            "Password changed successfully."
        ));
    }
}