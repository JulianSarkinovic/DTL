using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Entities.Owned
{
    public class EloHistory : Entity
    {
        private EloHistory()
        {
        }

        public EloHistory(int matchId, int eloId)
        {
            EloId = eloId;
            MatchId = matchId;
        }

        public int EloId { get; set; }
        public int MatchId { get; set; }
    }
}