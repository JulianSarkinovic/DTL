using DikkeTennisLijst.Core.Entities;
using System.Linq.Expressions;

namespace DikkeTennisLijst.Core.Shared.Specifications
{
    public class SpecificationClubsWithSurfaces : SpecificationBase<Club>
    {
        public SpecificationClubsWithSurfaces(Expression<Func<Club, bool>> criteria) : base(criteria)
        {
            AddInclude(c => c.SurfacesClubJoins!);
        }

        public SpecificationClubsWithSurfaces(Club entity) : base(x => x.Id == entity.Id)
        {
            AddInclude(c => c.SurfacesClubJoins!);
        }

        public SpecificationClubsWithSurfaces(int id) : base(x => x.Id == id)
        {
            AddInclude(c => c.SurfacesClubJoins!);
        }

        public SpecificationClubsWithSurfaces() : base(_ => true)
        {
            AddInclude(c => c.SurfacesClubJoins!);
        }
    }
}