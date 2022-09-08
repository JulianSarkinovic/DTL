namespace DikkeTennisLijst.Core.Shared.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException() : base("Login failed for provided username and password combination.")
        {
        }

        public LoginFailedException(string message) : base(message)
        {
        }

        public LoginFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}