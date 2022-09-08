using DikkeTennisLijst.Core.Shared.Enums;
using DikkeTennisLijst.Infrastructure.Configuration;
using System.ComponentModel.DataAnnotations;

namespace DikkeTennisLijst.WebAPI.Models.Request
{
    public class PlayerCreationRequestModel
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string[] Roles { get; set; } = { Role.Player };
        public Status Status { get; set; } = Status.Stub;
    }
}