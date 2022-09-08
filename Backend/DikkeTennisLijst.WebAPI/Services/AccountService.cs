using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Interfaces.Services;
using DikkeTennisLijst.Core.Services;
using DikkeTennisLijst.Core.Shared.Attributes;
using DikkeTennisLijst.Core.Shared.Enums;
using DikkeTennisLijst.Core.Shared.Helpers;
using DikkeTennisLijst.Core.Shared.Results;
using DikkeTennisLijst.WebAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DikkeTennisLijst.WebAPI.Services;

public class AccountService : ServiceBase, IAccountService
{
    private static string EmailPasswordCombinationInvalid => "The email password combination was invalid";

    private IEmailService EmailService { get; }
    private AppSettings AppSettings { get; }
    private UserManager<Player> UserManager { get; }
    private SignInManager<Player> SignInManager { get; }

    public AccountService(
        ILogger<PlayerService> logger,
        IEmailService emailService,
        IOptions<AppSettings> appSettings,
        UserManager<Player> userManager,
        SignInManager<Player> signInManager) : base(logger)
    {
        EmailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        AppSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    [Todo("This is probably not safe (too much information). Check OWASP.")]
    public async Task<OperationResult> ConfirmRegistrationAsync(string id, string token)
    {
        try
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return Failure(route: "user-not-found");
            }

            if (user.EmailConfirmed)
            {
                return Failure(route: "already-confirmed");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Confirmation of the email {USER.EMAIL} did not succeed.", user.Email);
                return Failure(result, "failed");
            }

            return Success(route: "success");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Failure(ex, route: "failed");
        }
    }

    [Todo("This is probably not safe (unlimited time tokens). Check OWASP.")]
    public async Task<OperationResult<(Player Player, List<string> Roles, string Token)>> AuthenticateAsync(
        string email, string password)
    {
        try
        {
            var user = await UserManager.FindByEmailAsync(email);

            if (user != null && user.Status != Status.Disabled)
            {
                var result = await SignInManager.CheckPasswordSignInAsync(user, password, true);

                if (result.Succeeded)
                {
                    var userRoles = await UserManager.GetRolesAsync(user);
                    var roles = userRoles.ToList();
                    var token = TokenHelper.CreateSecurityToken(user.Id, roles, AppSettings.JwtSecretKey);

                    return Success((user, roles, token));
                }
            }

            return Failure<(Player, List<string>, string)>(EmailPasswordCombinationInvalid);
        }
        catch (Exception ex)
        {
            return Failure<(Player, List<string>, string)>(ex, EmailPasswordCombinationInvalid);
        }
    }

    public async Task<OperationResult> ChangePasswordAsync(
        string email, string currentPassword, string newPassword)
    {
        try
        {
            var user = await UserManager.FindByEmailAsync(email);

            if (user != null)
            {
                var identityResult = await UserManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (identityResult.Succeeded)
                {
                    return Success();
                }
            }

            return Failure(EmailPasswordCombinationInvalid);
        }
        catch (Exception ex)
        {
            return Failure(ex, EmailPasswordCombinationInvalid);
        }
    }

    [Todo("Sign the user out (of all devices)?")]
    public async Task<OperationResult> ResetPasswordAsync(
        string email)
    {
        try
        {
            var user = await UserManager.FindByEmailAsync(email);

            if (user != null)
            {
                var token = await UserManager.GeneratePasswordResetTokenAsync(user);
                token = token.Replace("+", "%2B");

                await EmailService.SendPasswordResetEmailAsync(user, token);
            }

            return Success();
        }
        catch (Exception ex)
        {
            return Failure(ex, "Something went wrong on the server");
        }
    }

    public async Task<OperationResult<(Player Player, List<string> Roles, string Token)>> ResetPasswordConfirmAsync(
        string email, string newPassword, string token)
    {
        try
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await UserManager.ResetPasswordAsync(user, token, newPassword);
                if (result.Succeeded)
                {
                    return await AuthenticateAsync(email, newPassword);
                }
            }

            return Failure<(Player Player, List<string> Roles, string Token)>(EmailPasswordCombinationInvalid);
        }
        catch (Exception ex)
        {
            return Failure<(Player Player, List<string> Roles, string Token)>(ex, EmailPasswordCombinationInvalid);
        }
    }
}