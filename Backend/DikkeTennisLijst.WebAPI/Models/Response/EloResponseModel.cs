using DikkeTennisLijst.Core.Abstract;
using DikkeTennisLijst.Core.Entities.Owned;

namespace DikkeTennisLijst.WebAPI.Models.Response
{
    public class EloResponseModel : Entity
    {
        public int Rating { get; set; }
        public bool IsProvisional { get; set; }
        public ICollection<EloHistory> History { get; set; }
    }
}