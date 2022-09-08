using DikkeTennisLijst.Core.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace DikkeTennisLijst.WebAPI.Models.Request
{
    public class PlayerRequestModel
    {
        // Todo: reconsider using PUT to change a player, seeing the complexity behind the entity, patch may be the way to go;
        // If put stays used, make sure the PUT will update delta only for what the user CAN even send, PRequestM can not be more extensive than PResponseM;
        // Reconsider what is changed here.

        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public DateTimeOffset DateOfBirth { get; set; }

        [Required]
        public string[] Roles { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}