using DikkeTennisLijst.Core.Interfaces.Entities;
using DikkeTennisLijst.Core.Shared.Enums;

namespace DikkeTennisLijst.WebAPI.Models.Response
{
    public class PlayerResponseModel : IEntity<string>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string RegistrationNumber { get; set; }

        public string[] Roles { get; set; }
        public string Token { get; set; }

        public Gender Gender { get; set; }
        public Status Status { get; set; }
        public EloResponseModel Elo { get; set; }
    }
}