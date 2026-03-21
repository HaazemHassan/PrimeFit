using ErrorOr;
using NetTopologySuite.Geometries;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities.Base;
using PrimeFit.Domain.ValueObjects;

namespace PrimeFit.Domain.Entities
{
    public class Branch : FullAuditableEntity<int>
    {

        public const int MaxImageCount = 3;

        private Branch(int ownerId, string name, string email, string phoneNumber, BranchType branchType)
        {
            _workingHours = [];
            _reviews = [];
            _images = [];
            _packages = [];
            _subscriptions = [];
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
        public Point? Coordinates { get; private set; }



        private readonly List<BranchWorkingHour> _workingHours;
        public IReadOnlyCollection<BranchWorkingHour> WorkingHours => _workingHours;


        private readonly List<BranchReview> _reviews;
        public IReadOnlyCollection<BranchReview> Reviews => _reviews;


        public int OwnerId { get; private set; }
        public DomainUser Owner { get; private set; }



        private readonly List<BranchImage> _images;
        public IReadOnlyList<BranchImage> Images => _images.AsReadOnly();


        public IReadOnlyList<BranchImage> ActiveImages =>
           _images.Where(i => i.Status == BranchImageStatus.Active).ToList();
        public BranchImage? ActiveLogo => ActiveImages.FirstOrDefault(i => i.Type == BranchImageType.Logo);
        public IReadOnlyList<BranchImage> ActiveMarketPlaceImages =>
            ActiveImages.Where(i => i.Type == BranchImageType.MarketPlace).ToList();


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
            var coordinates = new Point(location.Longitude, location.Latitude)
            {
                SRID = 4326
            };

            Governorate = governorate;
            Address = address;
            Coordinates = coordinates;
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

        public bool IsOpenNow(DateTimeOffset currentDateTime)
        {

            if (_workingHours.Count == 0)
                return false;

            var today = currentDateTime.DayOfWeek;
            var yesterday = currentDateTime.AddDays(-1).DayOfWeek;

            foreach (var workingHour in _workingHours)
            {
                if (workingHour.IsClosed)
                    continue;

                var open = workingHour.OpenTime;
                var close = workingHour.CloseTime;

                var now = TimeOnly.FromTimeSpan(currentDateTime.TimeOfDay);

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


        public ErrorOr<Success> ActivateImage(BranchImage image)
        {
            if (!_images.Contains(image))
            {
                return Error.Validation(
                    description: "Image not found."
                );
            }


            if (image.Status != BranchImageStatus.Pending)
            {
                return Error.Validation(
                    description: "Only pending images can be activated."
                );
            }


            var imageToReplace = ActiveImages.FirstOrDefault(i => i.DisplayOrder == image.DisplayOrder);

            if (imageToReplace is not null)
            {
                imageToReplace!.SetStatus(BranchImageStatus.Replaced);

                image.SetStatus(BranchImageStatus.Active);

                return Result.Success;
            }


            if (ActiveMarketPlaceImages.Count > MaxImageCount)
            {
                return Error.Validation(
                    code: ErrorCodes.Branch.ImagesCountLimitExceeded,
                    description: $"Cannot add more than {MaxImageCount + 1} images to a branch."
                 );
            }

            image.SetStatus(BranchImageStatus.Active);
            return Result.Success;

        }

        public ErrorOr<BranchImage> AddImage(BranchImage image)
        {
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

            if (Coordinates is null)
                return Error.Validation(
                    code: ErrorCodes.Branch.LocationRequired,
                    description: "Branch location must be set to activate the branch."
                );

            if (_workingHours.Count == 0)
                return Error.Validation(
                    code: ErrorCodes.Branch.WorkingHoursRequired,
                    description: "Working hours must be defined before activating the branch."
                );

            if (ActiveLogo is null)
                return Error.Validation(
                    code: ErrorCodes.Branch.LogoRequired,
                    description: "A logo is required to activate the branch."
                );

            if (ActiveMarketPlaceImages.Count == 0)
                return Error.Validation(
                    code: ErrorCodes.Branch.MarketPlaceImagesRequired,
                    description: "At least one marketplace image is required to activate the branch."
                );

            return Result.Success;
        }

        #endregion


    }
}
