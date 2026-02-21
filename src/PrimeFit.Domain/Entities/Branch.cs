using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class Branch : FullAuditableEntity<int>
    {

        public Branch(int ownerId)
        {
            OwnerId = ownerId;
            Owner = null!;

            Name = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;

            _workingHours = new();
            _reviews = new();
            BranchStatus = BranchStatus.Draft;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public BranchType BranchType { get; private set; }
        public BranchStatus BranchStatus { get; private set; }



        public string? Address { get; private set; }
        public int? GovernorateId { get; private set; }
        public Governorate? Governorate { get; private set; }




        private List<BranchWorkingHour> _workingHours;
        public IReadOnlyCollection<BranchWorkingHour> WorkingHours => _workingHours;


        private List<BranchReview> _reviews;
        public IReadOnlyCollection<BranchReview> Reviews => _reviews;


        public int OwnerId { get; private set; }
        public DomainUser Owner { get; private set; }



        public void SetBussinessDetails(string branchName, string email, string phone, BranchType branchType)
        {
            Name = branchName;
            Email = email;
            PhoneNumber = phone;
            BranchType = branchType;
        }

        public void SetLocationDetails(Governorate governorate, string address)
        {
            Governorate = governorate;
            Address = address;
        }

        public void SetWorkingHours(List<BranchWorkingHour> branchWorkingHours)
        {
            _workingHours.Clear();

            foreach (var workingHours in branchWorkingHours)
            {
                _workingHours.Add(workingHours);
            }
        }


        public bool IsOwner(int userId)
        {
            return OwnerId == userId;
        }



    }
}
