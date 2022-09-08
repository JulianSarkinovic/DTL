using DikkeTennisLijst.Core.Entities;

namespace DikkeTennisLijst.Core.Shared.Specifications
{
    public class SpecificationPlayerWithMatches : SpecificationBase<Player>
    {
        public SpecificationPlayerWithMatches() : base(_ => true)
        {
            AddInclude(m => m.Matches ?? new List<Match>());
        }
    }
}