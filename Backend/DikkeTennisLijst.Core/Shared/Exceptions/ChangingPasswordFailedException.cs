namespace DikkeTennisLijst.Core.Shared.Exceptions
{
    public class ChangingPasswordFailedException : Exception
    {
        public ChangingPasswordFailedException() : base("Changing password failed.")
        {
        }

        public ChangingPasswordFailedException(string message) : base(message)
        {
        }

        public ChangingPasswordFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}