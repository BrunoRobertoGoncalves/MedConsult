namespace Domain.Core.Models
{
    public abstract class Entity<TKey> 
        where TKey : struct
    {
        public TKey Id { get; protected set; }
    }
}
