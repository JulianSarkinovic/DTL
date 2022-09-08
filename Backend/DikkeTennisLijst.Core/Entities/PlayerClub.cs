using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Entities
{
    public class PlayerClub : Entity
    {
#pragma warning disable CS8618

        private PlayerClub()
        {
        }

#pragma warning restore CS8618

        public PlayerClub(string playerId, int clubId)
        {
            PlayerId = playerId;
            ClubId = clubId;
        }

        public string PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        public int ClubId { get; set; }
        public Club Club { get; set; } = null!;
    }
}