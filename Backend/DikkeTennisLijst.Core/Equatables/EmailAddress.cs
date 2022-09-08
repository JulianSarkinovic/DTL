using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Equatables
{
    public class EmailAddress : Equatable<EmailAddress>
    {
        public string Email { get; set; }
        public string Name { get; set; }

        public EmailAddress(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public override int GetHashCode() =>
            (Email, Name).GetHashCode();

        public override bool IsEqual(EmailAddress other) =>
            Email == other.Email &&
            Name == other.Name;
    }
}