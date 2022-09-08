using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Entities
{
    public class Club : Entity
    {
#pragma warning disable CS8618

        private Club()
        {
        }

#pragma warning restore CS8618

        public Club(string name, string? registrationNumber = null)
        {
            Name = name;
            RegistrationNumber = registrationNumber;
        }

        public string Name { get; set; }

        /// <summary>
        /// The 'official' registration number, such as the KNLTB number in the Netherlands.
        /// </summary>
        public string? RegistrationNumber { get; set; }

        public ICollection<Match>? Matches { get; set; } = new List<Match>();
        public ICollection<PlayerClub>? PlayerClubs { get; set; } = new List<PlayerClub>();
        public ICollection<SurfaceClubJoin>? SurfacesClubJoins { get; set; } = new List<SurfaceClubJoin>();
        public ICollection<Address>? Addresses { get; set; } = new List<Address>();
    }
}