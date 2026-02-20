namespace PrimeFit.Domain.Entities.Contracts {
    public interface IHasCreator {
        int? CreatedBy { get; set; }
    }
}
