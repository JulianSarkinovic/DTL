namespace DikkeTennisLijst.Core.Shared.Exceptions
{
    public class SendEmailFailedException : Exception
    {
        public SendEmailFailedException() : base("Sending the email failed.")
        {
        }

        public SendEmailFailedException(Exception innerException) : base("Sending the email failed.", innerException)
        {
        }

        public SendEmailFailedException(string customMessage) : base(customMessage)
        {
        }

        public SendEmailFailedException(string customMessage, Exception innerException) : base(customMessage, innerException)
        {
        }
    }
}