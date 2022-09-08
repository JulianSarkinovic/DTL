using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Entities
{
    public class Comment : Entity
    {
#pragma warning disable CS8618

        private Comment()
        {
        }

#pragma warning restore CS8618

        public Comment(string text, string playerId, int matchId)
        {
            Text = text;
            PlayerId = playerId;
            MatchId = matchId;
        }

        public string Text { get; set; }
        public string PlayerId { get; set; }
        public int MatchId { get; set; }
    }
}