using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.Core.Entities
{
    public class EloCasual : Elo
    {
        public EloCasual()
        {
        }

        public EloCasual(string playerId) : base(playerId)
        {
        }
    }
}