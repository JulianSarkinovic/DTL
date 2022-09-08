using DikkeTennisLijst.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace DikkeTennisLijst.WebAPI.Models.Request
{
    public class ClubRequestModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int RegistrationNumber { get; set; }
        public IEnumerable<Surface> Surfaces { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
    }
}