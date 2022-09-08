using System.ComponentModel.DataAnnotations;

namespace DikkeTennisLijst.WebAPI.Models.Request
{
    public class MatchConfirmationDto
    {
        [Required]
        public int MatchId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public bool Agreed { get; set; }
    }
}