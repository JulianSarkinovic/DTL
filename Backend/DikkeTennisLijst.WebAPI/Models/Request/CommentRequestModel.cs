using System.ComponentModel.DataAnnotations;

namespace DikkeTennisLijst.WebAPI.Models.Request
{
    public class CommentRequestModel
    {
        public int Id { get; set; }

        [Required]
        public string PlayerId { get; set; }

        [Required]
        public int MatchId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}