using DikkeTennisLijst.Core.Abstract;

namespace DikkeTennisLijst.WebAPI.Models.Response
{
    public class ClubResponseModel : Entity
    {
        public string Name { get; set; }
        public int RegistrationNumber { get; set; }
        public int[] SurfacesIds { get; set; }
    }
}