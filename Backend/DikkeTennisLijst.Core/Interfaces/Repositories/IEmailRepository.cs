using DikkeTennisLijst.Core.Shared.Helpers;
using DikkeTennisLijst.Core.Shared.Exceptions;

namespace DikkeTennisLijst.Core.Interfaces.Repositories;

public interface IEmailRepository
{
    /// <summary>
    /// Sends the email.
    /// If the email is not succesfully sent, throws <see cref="SendEmailFailedException"/>.
    /// </summary>
    Task SendEmailAsync(EmailBasicData emailBasicData);
}