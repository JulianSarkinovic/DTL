using DikkeTennisLijst.Core.Interfaces.Entities;
using DikkeTennisLijst.Core.Shared.Enums;
using Microsoft.AspNetCore.Identity;

namespace DikkeTennisLijst.Core.Entities
{
    public class Player : IdentityUser, IEntity<string>, ITimestamps
    {
#pragma warning disable CS8618

        private Player()
        {
        }

#pragma warning restore CS8618

        public Player(string firstName, string lastName, string email) : this()
        {
            var now = DateTimeOffset.Now;
            CreatedAt = now;
            UpdatedAt = now;

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = email;
            NormalizedEmail = email.Normalize().ToUpperInvariant();
            NormalizedUserName = email.Normalize().ToUpperInvariant();

            EloRanked = new EloRanked();
            EloCasual = new EloCasual();
        }

        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        public Gender? Gender { get; set; }

        [PersonalData]
        public DateTimeOffset? DateOfBirth { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public Status Status { get; set; } = Status.Enabled;
        public ConfirmationStatus ConfirmationStatus { get; set; } = ConfirmationStatus.Awaiting;

        /// <summary>
        /// The 'official' registration number, such as the KNLTB number in the Netherlands.
        /// </summary>
        public int? RegistrationNumber { get; set; }

        public EloRanked EloRanked { get; set; }
        public EloCasual EloCasual { get; set; }
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
        public ICollection<Match>? Matches { get; set; } = new List<Match>();
        public ICollection<Friendship> Friendships { get; set; } = new List<Friendship>();
        public ICollection<Following>? Followings { get; set; } = new List<Following>();
        public ICollection<PlayerClub>? PlayerClubs { get; set; } = new List<PlayerClub>();
        public ICollection<Address>? Addresses { get; set; } = new List<Address>();

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({Id})";
        }
    }
}