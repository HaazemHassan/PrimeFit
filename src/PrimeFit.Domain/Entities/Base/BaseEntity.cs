namespace PrimeFit.Domain.Primitives
{
    namespace PrimeFit.Domain.Primitives
    {
        public abstract class BaseEntity<TId> : IEquatable<BaseEntity<TId>>, IHasDomainEvents where TId : notnull
        {
            private readonly List<IDomainEvent> _domainEvents = new();

            public TId Id { get; set; }

            public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

            protected void RaiseDomainEvent(IDomainEvent domainEvent)
            {
                _domainEvents.Add(domainEvent);
            }

            public void ClearDomainEvents()
            {
                _domainEvents.Clear();
            }

            public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right)
            {
                if (ReferenceEquals(left, right))
                    return true;

                if (left is null || right is null)
                    return false;

                return left.Equals(right);
            }

            public static bool operator !=(BaseEntity<TId>? left, BaseEntity<TId>? right)
                => !(left == right);

            public override bool Equals(object? obj)
                => Equals(obj as BaseEntity<TId>);

            public bool Equals(BaseEntity<TId>? other)
            {
                if (other is null)
                    return false;

                if (ReferenceEquals(this, other))
                    return true;

                if (GetType() != other.GetType())
                    return false;

                return EqualityComparer<TId>.Default.Equals(Id, other.Id);
            }

            public override int GetHashCode()
                => HashCode.Combine(GetType(), Id);
        }
    }

}
