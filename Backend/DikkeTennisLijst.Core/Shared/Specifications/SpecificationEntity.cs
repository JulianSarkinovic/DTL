using DikkeTennisLijst.Core.Interfaces.Entities;
using System.Linq.Expressions;

namespace DikkeTennisLijst.Core.Shared.Specifications
{
    public class SpecificationEntity<T> : SpecificationBase<T> where T : IEntity<int>
    {
        public SpecificationEntity(Expression<Func<T, bool>> criteria) : base(criteria)
        {
        }

        public SpecificationEntity(T entity) : base(x => x.Id == entity.Id)
        {
        }

        public SpecificationEntity(int id) : base(x => x.Id == id)
        {
        }
    }
}