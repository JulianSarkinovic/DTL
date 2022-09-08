using DikkeTennisLijst.Core.Shared.Specifications;
using DikkeTennisLijst.Tests.Common.Entities;
using System.Linq.Expressions;

namespace DikkeTennisLijst.Tests.Common.Specifications
{
    public class SpecificationTestEntity<T> : SpecificationBase<T> where T : IHasId
    {
        public SpecificationTestEntity(Expression<Func<T, bool>> criteria) : base(criteria)
        {
        }

        public SpecificationTestEntity(T entity) : base(x => x.Id == entity.Id)
        {
        }

        public SpecificationTestEntity(int id) : base(x => x.Id == id)
        {
        }
    }
}