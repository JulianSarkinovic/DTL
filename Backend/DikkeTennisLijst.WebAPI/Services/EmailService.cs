using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Equatables;
using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Services;
using DikkeTennisLijst.Core.Shared.Enums;
using DikkeTennisLijst.Core.Shared.Extensions;
using DikkeTennisLijst.Core.Shared.Helpers;
using Microsoft.Extensions.Options;

namespace DikkeTennisLijst.WebAPI.Services;

public class EmailService : IEmailService
{
    private static string BaseEmail => "BaseEmail.html";
    private static string AccountConfirmationEmail => "EmailAccountConfirmation.html";
    private static string MatchConfirmationEmail => "EmailMatchConfirmation.html";
    private static string PasswordResetEmail => "EmailResetPassword.html";
    private static string PathEmailTemplates { get; set; }
    private static Dictionary<string, string> EmailTemplates { get; set; }
    private static EmailAddress From { get; set; }

    private ITextFileRepository TextFileRepository { get; }
    private IEmailRepository EmailRepository { get; }
    private IWebHostEnvironment WebHostEnvironment { get; }
    private AppSettings AppSettings { get; }

    public EmailService(
        ITextFileRepository textFileRepository,
        IEmailRepository emailRepository,
        IWebHostEnvironment webHostEnvironment,
        IOptions<AppSettings> optionsAppsettings)
    {
        TextFileRepository = textFileRepository;
        EmailRepository = emailRepository;
        WebHostEnvironment = webHostEnvironment;
        AppSettings = optionsAppsettings.Value;
    }

    public async Task SendAccountConfirmationEmailAsync(Player user, string token)
    {
        await SetCacheAsync();

        var confirmationLink = $"{AppSettings.UrlFrontend}/email-confirmation?userId={user.Id}&token={token}";

        var confirmationEmail = EmailTemplates[AccountConfirmationEmail]
            .Replace("{name}", user.FirstName)
            .Replace("{link}", confirmationLink);

        var email = new EmailBasicData(
            GetEmailAddress(user), From, "Confirm your email", confirmationEmail, confirmationEmail);

        await EmailRepository.SendEmailAsync(email);
    }

    public async Task SendMatchConfirmationEmailAsync(Player user, Player opponent, Match match, bool isEdit)
    {
        await SetCacheAsync();

        var confirmationLink = $"{AppSettings.UrlFrontend}/match-confirmation?matchId={match.Id}&token={match.ConfirmationToken}";

        bool userIsOne = match.PlayerOneId.Equals(user.Id);
        bool oneWon = match.Winner == MatchWinner.PlayerOne;
        bool userWon = match.Winner != MatchWinner.Tie && ((userIsOne && oneWon) || (!userIsOne && !oneWon));

        var confirmationEmail = EmailTemplates[MatchConfirmationEmail]
            .Replace("{name_user}", opponent.FirstName)
            .Replace("{name_opponent}", $"{user.FirstName} {user.LastName}")
            .Replace("{action_opponent}", isEdit ? "editted the match you've registered" : "registered a new match")
            .Replace("{result_who}", match.Winner is MatchWinner.Tie ? "you tied" : userWon ? $"{user.FirstName} won" : "you've won")
            .Replace("{result_score}", match.Sets.ToReadableString(match.Winner))
            .Replace("{link}", confirmationLink);

        var email = new EmailBasicData(
            GetEmailAddress(opponent), From, $"Confirm your match with {user.FirstName}", confirmationEmail, confirmationEmail);

        await EmailRepository.SendEmailAsync(email);
    }

    public async Task SendPasswordResetEmailAsync(Player user, string token)
    {
        await SetCacheAsync();

        var confirmationLink = $"{AppSettings.UrlFrontend}/password-reset?token={token}";

        var confirmationEmail = EmailTemplates[PasswordResetEmail]
            .Replace("{name}", user.FirstName)
            .Replace("{link}", confirmationLink);

        var email = new EmailBasicData(
            GetEmailAddress(user), From, "Reset your password", confirmationEmail, confirmationEmail);

        await EmailRepository.SendEmailAsync(email);
    }

    private static EmailAddress GetEmailAddress(Player player)
    {
        return new EmailAddress(player.Email, $"{player.FirstName} {player.LastName}");
    }

    private async Task SetCacheAsync()
    {
        From ??= new EmailAddress(AppSettings.ApplicationEmail, AppSettings.ApplicationName);
        PathEmailTemplates ??= Path.Combine(WebHostEnvironment.WebRootPath, "assets", "emails");

        if (EmailTemplates is null)
        {
            EmailTemplates = new();
            EmailTemplates.Add(BaseEmail, await TextFileRepository.ReadHTMLAsync(PathEmailTemplates, BaseEmail));
            EmailTemplates.Add(AccountConfirmationEmail, await GetTemplateAsync(AccountConfirmationEmail));
            EmailTemplates.Add(MatchConfirmationEmail, await GetTemplateAsync(MatchConfirmationEmail));
            EmailTemplates.Add(PasswordResetEmail, await GetTemplateAsync(PasswordResetEmail));
        }
    }

    private async Task<string> GetTemplateAsync(string fileName)
    {
        var template = await TextFileRepository.ReadHTMLAsync(PathEmailTemplates, fileName);

        return EmailTemplates[BaseEmail]
            .Replace("{home}", AppSettings.UrlFrontend)
            .Replace("{logo}", "https://dikketennislijst.azurewebsites.net/assets/img/crown.png")
            .Replace("{body}", template);
    }
}