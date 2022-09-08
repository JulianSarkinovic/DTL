using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Shared.Exceptions;
using DikkeTennisLijst.Core.Shared.Helpers;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace DikkeTennisLijst.Infrastructure.Repositories;

public class SendGridRepository : IEmailRepository
{
    private readonly string SendGridKey;

    public SendGridRepository(IOptions<AppSettings> appSettings)
    {
        SendGridKey = appSettings.Value.SendGridKey;
    }

    public async Task SendEmailAsync(EmailBasicData emailBasicData)
    {
        await ExecuteAsync(
            emailBasicData.To.ConvertAll(e => new EmailAddress(e.Email, e.Name)),
            new EmailAddress(emailBasicData.From.Email, emailBasicData.From.Name),
            emailBasicData.Subject,
            emailBasicData.BodyAsText,
            emailBasicData.MessageHtml);
    }

    private async Task ExecuteAsync(
        List<EmailAddress> addresses,
        EmailAddress from,
        string subject,
        string messagePlain,
        string messageHtml)
    {
        var apiKey = SendGridKey;
        var client = new SendGridClient(apiKey);
        var email = new SendGridMessage()
        {
            From = from,
            Subject = subject,
            PlainTextContent = messagePlain,
            HtmlContent = messageHtml
        };

        foreach (var address in addresses)
        {
            email.AddTo(address);
        }

        var response = await client.SendEmailAsync(email);

        if (response.StatusCode is not HttpStatusCode.Accepted and not HttpStatusCode.OK)
        {
            throw new SendEmailFailedException();
        }
    }
}