using DikkeTennisLijst.Core.Equatables;

namespace DikkeTennisLijst.Core.Shared.Helpers
{
    public class EmailBasicData
    {
        public EmailBasicData(IEnumerable<EmailAddress> to, EmailAddress from, string subject, string messagePlain, string messageHtml)
        {
            To = to.ToList();
            From = from;
            Subject = subject;
            BodyAsText = messagePlain;
            MessageHtml = messageHtml;
        }

        public EmailBasicData(EmailAddress to, EmailAddress from, string subject, string messagePlain, string messageHtml)
        {
            To = new List<EmailAddress>() { to };
            From = from;
            Subject = subject;
            BodyAsText = messagePlain;
            MessageHtml = messageHtml;
        }

        public List<EmailAddress> To { get; init; }
        public EmailAddress From { get; init; }
        public string Subject { get; init; }
        public string BodyAsText { get; init; }
        public string MessageHtml { get; init; }
    }
}