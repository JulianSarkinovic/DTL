using DikkeTennisLijst.Core.Interfaces.Entities;
using DikkeTennisLijst.Core.Shared.Enums;

namespace DikkeTennisLijst.WebAPI.Models.Response
{
    public class PlayerForProfileResponseModel : IEntity<string>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public EloResponseModel Elo { get; set; }
        public List<MatchFullResponseModel> Matches { get; set; }
    }
}