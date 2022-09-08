using DikkeTennisLijst.Core.Abstract;
using DikkeTennisLijst.Core.Shared.Attributes;

namespace DikkeTennisLijst.Core.Equatables
{
    public class EmailTemplate : Equatable<EmailTemplate>
    {
#pragma warning disable CS8618

        // Info: This block is here for Serializer purposes;
        public EmailTemplate()
        {
        }

#pragma warning restore CS8618

        public EmailTemplate(
            string templateFileName,
            string subject,
            string bodyAsText,
            string bodyAsHTML,
            EmailAddress from)
        {
            Subject = subject;
            BodyAsText = bodyAsText;
            BodyAsHTML = bodyAsHTML;
            TemplateFileName = templateFileName;
            From = from;
        }

        public EmailTemplate(
            string templateFileName,
            string subject,
            string bodyAsText,
            string bodyAsHTML,
            string fromEmail = "j.sarkinovic@betabit.nl",
            string fromName = "The Tennis List")
        {
            Subject = subject;
            BodyAsText = bodyAsText;
            BodyAsHTML = bodyAsHTML;
            TemplateFileName = templateFileName;
            From = new EmailAddress(fromEmail, fromName);
        }

        public string TemplateFileName { get; set; }
        public EmailAddress From { get; set; }
        public string Subject { get; set; }
        public string BodyAsText { get; set; }
        public string BodyAsHTML { get; set; }

        public override bool IsEqual(EmailTemplate other)
        {
            return TemplateFileName == other.TemplateFileName;
        }

        [Todo("Remove when the BlobStorageRepository is not any more dependent on hash codes.")]
        public override int GetHashCode()
        {
            return GetStableHashCode(TemplateFileName);
        }

        private int GetStableHashCode(string str)
        {
            unchecked
            {
                int hash1 = 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1 || str[i + 1] == '\0')
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }
    }
}