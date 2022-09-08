using System.Runtime.Serialization;

namespace DikkeTennisLijst.Core.Shared.Exceptions
{
    /// <summary>
    /// The <see cref="Exception"/> that should be thrown if an element is added to storage while it is already there.
    /// </summary>
    public class DuplicateElementException : Exception
    {
        public DuplicateElementException()
        {
        }

        public DuplicateElementException(string message) : base(message)
        {
        }

        protected DuplicateElementException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DuplicateElementException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static DuplicateElementException New<T>(T obj, Exception innerException)
        {
            return new DuplicateElementException($"{typeof(T).Name} {obj?.GetHashCode()} already exists.", innerException);
        }
    }
}