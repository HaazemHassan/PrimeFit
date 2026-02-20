namespace PrimeFit.Domain.Entities.Contracts {
    public interface IAuditableEntity : IHasCreationTime, IHasCreator, IHasModificationTime, IHasModifier {

    }
}
