using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.WebAPI.Models.Response
{
    public class CommentResponseModel : Entity
    {
        public int MatchId { get; set; }
        public string PlayerId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}