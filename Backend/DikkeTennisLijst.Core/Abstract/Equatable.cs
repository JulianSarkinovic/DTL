namespace DikkeTennisLijst.Core.Abstract
{
    public abstract class Equatable<T> : IEquatable<T> where T : Equatable<T>
    {
        public override sealed bool Equals(object? other)
        {
            return ReferenceEquals(this, other)
                || (other is not null && other.GetType() == GetType() && IsEqual((T)other));
        }

        public bool Equals(T? other)
        {
            return ReferenceEquals(this, other)
                || (other is not null && IsEqual(other));
        }

        public static bool operator ==(Equatable<T> value, Equatable<T> other)
        {
            return ReferenceEquals(value, other)
                || (value is not null && other is not null && ((T)value).IsEqual((T)other));
        }

        public static bool operator !=(Equatable<T> value, Equatable<T> other)
        {
            return !(value == other);
        }

        public abstract override int GetHashCode();

        /// <summary>
        /// Do not call this method - call <see cref="Equals(T?)"/> instead.
        /// Override and provide a simple check for equality between two not null <typeparamref name="T"/>s.
        /// </summary>
        public abstract bool IsEqual(T other);
    }
}