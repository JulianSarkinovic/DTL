using DikkeTennisLijst.Core.Interfaces.Entities;
using System.Linq.Expressions;

namespace DikkeTennisLijst.Core.Shared.Specifications
{
    public abstract class SpecificationBase<T> : ISpecification<T>
    {
        protected SpecificationBase(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }
}