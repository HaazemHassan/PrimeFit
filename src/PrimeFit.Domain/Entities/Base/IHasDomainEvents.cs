namespace PrimeFit.Domain.Primitives.PrimeFit.Domain.Primitives
{
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
