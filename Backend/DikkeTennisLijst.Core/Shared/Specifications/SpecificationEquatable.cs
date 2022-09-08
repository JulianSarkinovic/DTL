using System.Linq.Expressions;

namespace DikkeTennisLijst.Core.Shared.Specifications
{
    public class SpecificationEquatable<T> : SpecificationBase<T> where T : IEquatable<T>
    {
        public SpecificationEquatable(Expression<Func<T, bool>> criteria) : base(criteria)
        {
        }

        public SpecificationEquatable(T equatable) : base(x => x.Equals(equatable))
        {
        }
    }
}