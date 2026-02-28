namespace PrimeFit.Domain.Entities.Contracts {
    public interface IHasModificationTime {
        DateTimeOffset? UpdatedAt { get; set; }
    }
}
