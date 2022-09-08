using DikkeTennisLijst.Core.Interfaces.Entities;

namespace DikkeTennisLijst.Core.Shared.Exceptions
{
    /// <summary>
    /// The <see cref="Exception"/> that should be thrown when trying to deal with an element that does not exist.
    /// </summary>
    public class NonExistentElementException : Exception
    {
        public NonExistentElementException()
        {
        }

        public NonExistentElementException(string? message) : base(message)
        {
        }

        public NonExistentElementException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public static NonExistentElementException New<T>(T obj)
        {
            return New<T>(obj?.GetHashCode() ?? -666);
        }

        public static NonExistentElementException New<T>(int id, ISpecification<T> spec)
        {
            return new NonExistentElementException($"The {typeof(T).Name} with ID {id} and criteria {spec.Criteria} does not exist.");
        }

        public static NonExistentElementException New<T>(ISpecification<T> spec)
        {
            return new NonExistentElementException($"The {typeof(T).Name} with criteria {spec.Criteria} does not exist.");
        }

        public static NonExistentElementException New<T>(int id)
        {
            return new NonExistentElementException($"The {typeof(T).Name} with ID {id} does not exist.");
        }

        public static NonExistentElementException New<T>(Exception innerException)
        {
            return new NonExistentElementException($"The {typeof(T).Name} sought does not exist.", innerException);
        }
    }
}