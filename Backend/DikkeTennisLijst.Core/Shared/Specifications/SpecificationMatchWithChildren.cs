using DikkeTennisLijst.Core.Entities;

namespace DikkeTennisLijst.Core.Shared.Specifications
{
    public class SpecificationMatchWithChildren : SpecificationBase<Match>
    {
        public SpecificationMatchWithChildren() : base(_ => true)
        {
            AddInclude(m => m.Club!);
            AddInclude(m => m.Surface);
            AddInclude(m => m.PlayerOne);
            AddInclude(m => m.PlayerTwo);
            AddInclude(m => m.PlayerOnePartner!);
            AddInclude(m => m.PlayerTwoPartner!);
        }
    }
}