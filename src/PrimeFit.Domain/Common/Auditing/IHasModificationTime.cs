namespace PrimeFit.Domain.Common.Auditing {
    public interface IHasModificationTime {
        DateTime? UpdatedAt { get; set; }
    }
}
