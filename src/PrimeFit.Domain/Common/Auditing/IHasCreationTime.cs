namespace PrimeFit.Domain.Common.Auditing {
    public interface IHasCreationTime {
        DateTime CreatedAt { get; set; }
    }
}
