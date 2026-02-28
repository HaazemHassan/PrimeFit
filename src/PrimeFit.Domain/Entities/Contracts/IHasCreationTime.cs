namespace PrimeFit.Domain.Entities.Contracts {
    public interface IHasCreationTime {
        DateTimeOffset CreatedAt { get; set; }
    }
}
