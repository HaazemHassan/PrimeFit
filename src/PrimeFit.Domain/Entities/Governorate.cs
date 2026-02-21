using PrimeFit.Domain.Primitives.PrimeFit.Domain.Primitives;

namespace PrimeFit.Domain.Entities
{
    public class Governorate : BaseEntity<int>
    {
        public Governorate(string name)
        {
            Name = name;
        }

        public Governorate(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; private set; }
    }
}
