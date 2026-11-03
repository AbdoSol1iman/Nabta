using Nabta.Application.Dtos.Auth;
namespace Nabta.Application.Interfaces.Services
{   public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterDto dto);

        Task<AuthModel> LoginAsync(LoginDto loginDto);

        Task SignOut();

        Task<AuthModel> RefreshTokenAsync(string token);

        Task<bool> RevokeToken(string token);

        // Email and password actions
        Task<bool> ConfirmEmailAsync(string userId, string token);

        Task ForgotPasswordAsync(string email);

        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);

    }
}