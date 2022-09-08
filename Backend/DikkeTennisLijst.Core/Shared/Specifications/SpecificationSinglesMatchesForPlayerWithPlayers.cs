using DikkeTennisLijst.Core.Entities;

namespace DikkeTennisLijst.Core.Shared.Specifications
{
    public class SpecificationSinglesMatchesForPlayerWithPlayers : SpecificationBase<Match>
    {
        public SpecificationSinglesMatchesForPlayerWithPlayers(string id)
            : base(x => x.PlayerOneId.Equals(id) || x.PlayerTwoId.Equals(id))
        {
            AddInclude(m => m.PlayerOne);
            AddInclude(m => m.PlayerTwo);
        }
    }
}