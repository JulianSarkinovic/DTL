using DikkeTennisLijst.Core.Abstract;
using DikkeTennisLijst.Core.Entities.Owned;
using DikkeTennisLijst.Core.Shared.Enums;

namespace DikkeTennisLijst.WebAPI.Models.Response
{
    public class MatchFullResponseModel : Entity
    {
        public string PlayerOneId { get; set; }
        public string PlayerOneFirstName { get; set; }
        public string PlayerOneLastName { get; set; }

        public string PlayerTwoId { get; set; }
        public string PlayerTwoFirstName { get; set; }
        public string PlayerTwoLastName { get; set; }

        public string PlayerOnePartnerId { get; set; }
        public string PlayerOnePartnerFirstName { get; set; }
        public string PlayerOnePartnerLastName { get; set; }

        public string PlayerTwoPartnerId { get; set; }
        public string PlayerTwoPartnerFirstName { get; set; }
        public string PlayerTwoPartnerLastName { get; set; }

        public SurfaceResponseModel Surface { get; set; }
        public ClubResponseModel Club { get; set; }

        public ICollection<Set> Sets { get; set; }
        public Duration Duration { get; set; }
        public MatchWinner Winner { get; set; }
        public MatchFormat Format { get; set; }
        public Core.Shared.Enums.MatchType Type { get; set; }
        public ConfirmationStatus ConfirmationStatus { get; set; }
        public bool Ranked { get; set; }
    }
}