using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Shared.Results;

namespace DikkeTennisLijst.Core.Interfaces.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// Finalizes the registration of the user.
        /// This follows on the confirmation of the registration/email by the user.
        /// </summary>
        Task<OperationResult> ConfirmRegistrationAsync(string id, string token);

        /// <summary>
        /// Authenticates the user by <paramref name="email"/> <paramref name="password"/> combination.
        /// </summary>
        Task<OperationResult<(Player Player, List<string> Roles, string Token)>> AuthenticateAsync(string email, string password);

        /// <summary>
        /// Changes the password of the user to the <paramref name="newPassword"/>,
        /// authenticated with his <paramref name="currentPassword"/>.
        /// </summary>
        Task<OperationResult> ChangePasswordAsync(string email, string newPassword, string currentPassword);

        /// <summary>
        /// Sends an email to the user with a token that can be used to authenticate the password reset request.
        /// </summary>
        Task<OperationResult> ResetPasswordAsync(string email);

        /// <summary>
        /// Changes the password of the user to the <paramref name="newPassword"/>,
        /// authenticated with the <paramref name="token"/>.
        /// </summary>
        Task<OperationResult<(Player Player, List<string> Roles, string Token)>> ResetPasswordConfirmAsync(
            string email, string newPassword, string token);
    }
}