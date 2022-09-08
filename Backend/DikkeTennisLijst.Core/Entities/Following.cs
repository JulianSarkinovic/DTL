using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Entities
{
    public class Following : Entity
    {
#pragma warning disable CS8618

        private Following()
        {
        }

#pragma warning restore CS8618

        public Following(string followerId, string followedId)
        {
            FollowerId = followerId;
            FollowedId = followedId;
        }

        public string FollowerId { get; set; }
        public Player Follower { get; set; } = null!;
        public string FollowedId { get; set; }
        public Player Followed { get; set; } = null!;
    }
}