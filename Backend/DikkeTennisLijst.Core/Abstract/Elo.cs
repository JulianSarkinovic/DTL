using DikkeTennisLijst.Core.Entities.Owned;
using DikkeTennisLijst.Core.Interfaces.Entities;
using DikkeTennisLijst.Core.Shared.Constants;
using System.Collections.Generic;

namespace DikkeTennisLijst.Core.Abstract
{
    public abstract class Elo : Entity, IElo
    {
#pragma warning disable CS8618

        protected Elo()
        {
        }

#pragma warning restore CS8618

        protected Elo(string playerId)
        {
            PlayerId = playerId;
        }

        public int Rating { get; set; } = EloConstants.StartElo;
        public int K { get; set; } = EloConstants.StartK;
        public bool IsProvisional { get; set; } = true;
        public string PlayerId { get; set; }

        /// <summary>
        /// A collection of EloHistory for history purposes.
        /// Match {Match: Match, MatchId: int, Elo: int}
        /// </summary>
        public ICollection<EloHistory> History { get; set; } = new List<EloHistory>();
    }
}