using Application.Features.Authentication.Register;
using Application.Features.Authentication.RevokeToken;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Nabta.Application.Dtos.Auth;
using Nabta.Application.Featuers;
using Nabta.Application.Featuers.Authentication.ConfirmEmail;
using Nabta.Application.Featuers.Authentication.ForgetPassword;
using Nabta.Application.Featuers.Authentication.LogIn;
using Nabta.Application.Featuers.Authentication.ResetPassword;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register( [FromBody] RegisterDto dto)
    {
        var result = await _mediator.Send( new RegisterCommand(dto));

        if (!result.IsAuthenticated)
        {
            return BadRequest(new
            {
                result.Message
            });
        }

        SetRefreshTokenCookie(
            result.RefreshToken,
            result.RefreshTokenExpiration);

        // Do not expose the refresh token in the response body.
        result.RefreshToken = null;

        return Ok(result);
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginDto dto)
    {
        var result = await _mediator.Send(
            new LogInCommand(dto));

        if (!result.IsAuthenticated)
        {
            return Unauthorized(new
            {
                result.Message
            });
        }

        SetRefreshTokenCookie(
            result.RefreshToken,
            result.RefreshTokenExpiration);

        result.RefreshToken = null;

        return Ok(result);
    }

    // POST: api/auth/refresh-token
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return Unauthorized(new
            {
                Message = "Refresh token is missing."
            });
        }

        var result = await _mediator.Send(
            new RefreshTokenCommand(refreshToken));

        if (!result.IsAuthenticated)
        {
            DeleteRefreshTokenCookie();

            return Unauthorized(new
            {
                result.Message
            });
        }

        if (string.IsNullOrWhiteSpace(result.RefreshToken))
        {
            DeleteRefreshTokenCookie();

            return Unauthorized(new
            {
                Message = "A new refresh token was not generated."
            });
        }

        SetRefreshTokenCookie(
            result.RefreshToken,
            result.RefreshTokenExpiration);

        result.RefreshToken = null;

        return Ok(result);
    }

    // POST: api/auth/revoke-token
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return Unauthorized(new
            {
                Message = "Refresh token is missing."
            });
        }

        var revoked = await _mediator.Send(
            new RevokeTokenCommand(refreshToken));

        if (!revoked)
        {
            return BadRequest(new
            {
                Message = "Refresh token is invalid or inactive."
            });
        }

        DeleteRefreshTokenCookie();

        return NoContent();
    }


    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result)
            return BadRequest("Invalid confirmation request.");

        return Ok("Email confirmed successfully.");
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new{ Message = "If your email is registered, you will receive a reset link."});
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result)
            return BadRequest("Invalid reset request.");

        return Ok("Password reset successfully.");
    }

    private void SetRefreshTokenCookie(string? token, DateTime? expiresOn)
    {
        if (string.IsNullOrWhiteSpace(token) || expiresOn is null)
            return;

        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expiresOn.Value,
            Path = "/api/auth"
        };

        Response.Cookies.Append(
            "refreshToken",
            token,
            options);
    }

    private void DeleteRefreshTokenCookie()
    {
        Response.Cookies.Delete(
            "refreshToken",
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/auth"
            });
    }
}
