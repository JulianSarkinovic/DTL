using DikkeTennisLijst.Core.Abstract;
using DikkeTennisLijst.Core.Entities.Owned;
using DikkeTennisLijst.Core.Shared.Enums;

namespace DikkeTennisLijst.WebAPI.Models.Response
{
    public class MatchResponseModel : Entity
    {
        public string PlayerOneId { get; set; }
        public PlayerResponseModel PlayerOne { get; set; }

        public string PlayerTwoId { get; set; }
        public PlayerResponseModel PlayerTwo { get; set; }

        public string PlayerOnePartnerId { get; set; }
        public PlayerResponseModel PlayerOnePartner { get; set; }

        public string PlayerTwoPartnerId { get; set; }
        public PlayerResponseModel PlayerTwoPartner { get; set; }

        public int SurfaceId { get; set; }
        public int? ClubId { get; set; }

        public ICollection<Set> Sets { get; set; }
        public Duration Duration { get; set; }
        public MatchWinner Winner { get; set; }
        public MatchFormat Format { get; set; }
        public Core.Shared.Enums.MatchType Type { get; set; }
        public bool Ranked { get; set; }
    }
}