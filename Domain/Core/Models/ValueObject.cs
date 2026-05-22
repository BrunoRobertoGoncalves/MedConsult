using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Models
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            return ((ValueObject)obj)
                .GetEqualityComponents()
                .SequenceEqual(GetEqualityComponents());
        }

        public override int GetHashCode()
            => GetEqualityComponents()
                .Select(x => x?.GetHashCode() ?? 0)
                .Aggregate((x, y) => x ^ y);

        public static bool operator ==(ValueObject? a, ValueObject? b)
            => a?.Equals(b) ?? b is null;

        public static bool operator !=(ValueObject? a, ValueObject? b)
            => !(a == b);
    }
}
