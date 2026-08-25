namespace Domain.Core.Models
{
    public abstract class Entity<TKey>
        where TKey : struct
    {
        public TKey Id { get; protected set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TKey> other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            if (Id.Equals(default(TKey)) || other.Id.Equals(default(TKey)))
                return false;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
            => HashCode.Combine(GetType(), Id);

        public static bool operator ==(Entity<TKey>? a, Entity<TKey>? b)
            => a?.Equals(b) ?? b is null;

        public static bool operator !=(Entity<TKey>? a, Entity<TKey>? b)
            => !(a == b);
    }
}
