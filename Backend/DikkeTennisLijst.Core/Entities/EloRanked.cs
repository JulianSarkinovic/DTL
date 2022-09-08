using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Entities
{
    public class EloRanked : Elo
    {
        public EloRanked()
        {
        }

        public EloRanked(string playerId) : base(playerId)
        {
        }
    }
}