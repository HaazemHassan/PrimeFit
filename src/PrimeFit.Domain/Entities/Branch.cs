using ErrorOr;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities.Base;
using PrimeFit.Domain.ValueObjects;

namespace PrimeFit.Domain.Entities
{
    public class Branch : FullAuditableEntity<int>
    {

        public const int MaxImageCount = 6;

        private Branch(int ownerId, string name, string email, string phoneNumber, BranchType branchType)
        {
            _workingHours = new();
            _reviews = new();
            _images = new();
            _packages = new();
            _subscriptions = new();
            BranchStatus = BranchStatus.Inactive;


            OwnerId = ownerId;
            Owner = null!;

            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            BranchType = branchType;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public BranchType BranchType { get; private set; }
        public BranchStatus BranchStatus { get; private set; }



        public int? GovernorateId { get; private set; }
        public Governorate? Governorate { get; private set; }
        public string? Address { get; private set; }
        public GeoLocation? Location { get; private set; }



        private readonly List<BranchWorkingHour> _workingHours;
        public IReadOnlyCollection<BranchWorkingHour> WorkingHours => _workingHours;


        private readonly List<BranchReview> _reviews;
        public IReadOnlyCollection<BranchReview> Reviews => _reviews;


        public int OwnerId { get; private set; }
        public DomainUser Owner { get; private set; }



        private readonly List<BranchImage> _images;
        public IReadOnlyList<BranchImage> Images => _images.AsReadOnly();
        public BranchImage? Logo => _images.FirstOrDefault(i => i.Type == BranchImageType.Logo);
        public IReadOnlyList<BranchImage> MarketPlaceImages =>
            _images.Where(i => i.Type == BranchImageType.MarketPlace).ToList();


        private readonly List<Package> _packages;
        public IReadOnlyList<Package> Packages => _packages.AsReadOnly();

        private readonly List<Subscription> _subscriptions;
        public IReadOnlyList<Subscription> Subscriptions => _subscriptions.AsReadOnly();







        public static ErrorOr<Branch> Create(int ownerId, string branchName, string email, string phoneNumber, BranchType branchType)
        {
            if (ownerId <= 0)
            {
                return Error.Validation(description: "OwnerId is required.");

            }
            return new Branch(ownerId, branchName, email, phoneNumber, branchType);

        }



        public void UpdateBasicDetails(string? name, string? email, string? phoneNumber, BranchType? branchType)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Name = name;
            }

            if (!string.IsNullOrEmpty(email))
            {
                Email = email;
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                PhoneNumber = phoneNumber;
            }

            if (branchType.HasValue)
            {
                BranchType = branchType.Value;
            }

        }

        public void UpdateLocationDetails(Governorate governorate, string address, GeoLocation location)
        {
            Governorate = governorate;
            Address = address;
            Location = location;
        }

        public void UpdateWorkingHours(List<BranchWorkingHour> branchWorkingHours)
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

        public bool IsOpenNow(DateTime currentDateTime)
        {

            if (!_workingHours.Any())
                return false;

            var today = currentDateTime.DayOfWeek;
            var yesterday = currentDateTime.AddDays(-1).DayOfWeek;

            foreach (var workingHour in _workingHours)
            {
                if (workingHour.IsClosed)
                    continue;

                var open = workingHour.OpenTime;
                var close = workingHour.CloseTime;
                var now = TimeOnly.FromDateTime(currentDateTime);

                bool crossesMidnight = close <= open;

                if (!crossesMidnight)
                {
                    if (workingHour.Day == today &&
                        now >= open &&
                        now < close)
                        return true;
                }
                else
                {
                    if (
                        (workingHour.Day == today && now >= open) ||
                        (workingHour.Day == yesterday && now < close)
                       )
                        return true;
                }
            }

            return false;
        }



        public ErrorOr<Success> CanAddImage(BranchImageType type)
        {
            if (_images.Count > MaxImageCount)
            {
                return Error.Validation(
                    code: ErrorCodes.Branch.ImagesCountLimitExceeded,
                    description: $"Cannot add more than {MaxImageCount} images to a branch."
                 );
            }

            if (type == BranchImageType.Logo && _images.Any(i => i.Type == BranchImageType.Logo))
            {
                return Error.Conflict(
                    code: ErrorCodes.Branch.LogoAlreadyExists,
                    description: "Branch already has a logo"
                 );

            }

            return Result.Success;

        }

        public ErrorOr<BranchImage> AddImage(string url, string publicId, BranchImageType type)
        {
            var canAddImageResult = CanAddImage(type);
            if (canAddImageResult.IsError)
                return canAddImageResult.Errors;

            var image = BranchImage.Create(url, publicId, type, Id);

            _images.Add(image);

            return image;
        }
        public ErrorOr<string> DeleteImage(int imageId)
        {
            var image = _images.FirstOrDefault(i => i.Id == imageId);

            if (image is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Branch.ImageNotFound,
                    description: "Image not found."
                );
            }


            _images.Remove(image);

            return image.PublicId;
        }

        public ErrorOr<Package> AddPackage(string name, decimal price, int durationInMonths, bool isActive, int numberOfFreezes, int freezeDurationInDays)
        {
            var package = new Package
            {
                BranchId = Id,
                Name = name,
                Price = price,
                DurationInMonths = durationInMonths,
                IsActive = isActive,
                NumberOfFreezes = numberOfFreezes,
                FreezeDurationInDays = freezeDurationInDays
            };

            _packages.Add(package);

            return package;
        }



        public ErrorOr<Success> Activate()
        {

            if (BranchStatus == BranchStatus.Active)
                return Result.Success;

            var canBeActivatedResult = EnsureCanBeActivated();
            if (canBeActivatedResult.IsError)
            {
                return canBeActivatedResult.Errors;
            }

            BranchStatus = BranchStatus.Active;
            return Result.Success;

        }


        public ErrorOr<Success> DeActivate()
        {
            BranchStatus = BranchStatus.Inactive;
            return Result.Success;

        }


        #region Helpers
        private ErrorOr<Success> EnsureCanBeActivated()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return Error.Validation(
                    code: ErrorCodes.Branch.NameRequired,
                    description: "Branch name is required to activate the branch."
                );

            if (string.IsNullOrWhiteSpace(Email))
                return Error.Validation(
                    code: ErrorCodes.Branch.EmailRequired,
                    description: "Branch email is required to activate the branch."
                );

            if (string.IsNullOrWhiteSpace(PhoneNumber))
                return Error.Validation(
                    code: ErrorCodes.Branch.PhoneRequired,
                    description: "Branch phone number is required to activate the branch."
                );

            if (GovernorateId is null)
                return Error.Validation(
                    code: ErrorCodes.Branch.GovernorateRequired,
                    description: "Governorate must be selected to activate the branch."
                );

            if (string.IsNullOrWhiteSpace(Address))
                return Error.Validation(
                    code: ErrorCodes.Branch.AddressRequired,
                    description: "Branch address is required to activate the branch."
                );

            if (Location is null)
                return Error.Validation(
                    code: ErrorCodes.Branch.LocationRequired,
                    description: "Branch location must be set to activate the branch."
                );

            if (_workingHours.Count == 0)
                return Error.Validation(
                    code: ErrorCodes.Branch.WorkingHoursRequired,
                    description: "Working hours must be defined before activating the branch."
                );

            if (Logo is null)
                return Error.Validation(
                    code: ErrorCodes.Branch.LogoRequired,
                    description: "A logo is required to activate the branch."
                );

            if (MarketPlaceImages.Count == 0)
                return Error.Validation(
                    code: ErrorCodes.Branch.MarketPlaceImagesRequired,
                    description: "At least one marketplace image is required to activate the branch."
                );

            return Result.Success;
        }

        #endregion


    }
}
