using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Shared.Exceptions;

namespace DikkeTennisLijst.Core.Services;

public interface IEmailService
{
    /// <summary>
    /// Sends the account confirmation email.
    /// If the email is not succesfully sent, throws an <see cref="SendEmailFailedException"/>.
    /// </summary>
    public Task SendAccountConfirmationEmailAsync(Player user, string token);

    /// <summary>
    /// Sends the match confirmation email.
    /// If the email is not succesfully sent, throws an <see cref="SendEmailFailedException"/>.
    /// </summary>
    public Task SendMatchConfirmationEmailAsync(Player user, Player opponent, Match match, bool isEdit);

    /// <summary>
    /// Sends the password reset email.
    /// If the email is not succesfully sent, throws an <see cref="SendEmailFailedException"/>.
    /// </summary>
    public Task SendPasswordResetEmailAsync(Player user, string token);
}