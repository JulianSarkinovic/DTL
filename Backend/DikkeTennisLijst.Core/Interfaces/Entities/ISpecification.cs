﻿using System.Linq.Expressions;

namespace DikkeTennisLijst.Core.Interfaces.Entities
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
    }
}