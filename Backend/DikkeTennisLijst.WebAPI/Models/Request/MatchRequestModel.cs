using DikkeTennisLijst.Core.Entities.Owned;
using DikkeTennisLijst.Core.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace DikkeTennisLijst.WebAPI.Models.Request
{
    public class MatchRequestModel
    {
        public int Id { get; set; }

        [Required]
        public string PlayerOneId { get; set; }

        [Required]
        public string PlayerTwoId { get; set; }

        [Required]
        public ICollection<Set> Sets { get; set; }

        [Required]
        public Core.Shared.Enums.MatchType Type { get; set; }

        [Required]
        public MatchWinner Winner { get; set; }

        [Required]
        public bool Ranked { get; set; }

        [Required]
        public Duration Duration { get; set; }

        [Required]
        public int SurfaceId { get; set; }

        public MatchFormat Format { get; set; }
        public Status Status { get; set; } = Status.Enabled;
        public ConfirmationStatus ConfirmationStatus { get; set; } = ConfirmationStatus.Awaiting;
        public string PlayerOnePartnerId { get; set; }
        public string PlayerTwoPartnerId { get; set; }
        public int? ClubId { get; set; }
    }
}