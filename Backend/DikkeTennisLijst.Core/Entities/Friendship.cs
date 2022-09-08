using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Entities
{
    public class Friendship : Entity
    {
#pragma warning disable CS8618

        private Friendship()
        {
        }

#pragma warning restore CS8618

        public Friendship(string frienderId, string friendedId)
        {
            FrienderId = frienderId;
            FriendedId = friendedId;
        }

        public string FrienderId { get; set; }
        public Player Friender { get; set; } = null!;
        public string FriendedId { get; set; }
        public Player Friended { get; set; } = null!;
    }
}