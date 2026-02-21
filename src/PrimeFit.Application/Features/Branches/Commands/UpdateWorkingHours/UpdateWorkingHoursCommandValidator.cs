using FluentValidation;

namespace PrimeFit.Application.Features.Branches.Commands.AddWorkingHours
{
    public class UpdateWorkingHoursCommandValidator : AbstractValidator<UpdateWorkingHoursCommand>
    {
        public UpdateWorkingHoursCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.BranchId)
                .GreaterThan(0)
                .WithMessage("BranchId must be greater than 0");

            RuleFor(x => x.WorkingHours)
                .NotEmpty()
                .WithMessage("Working hours list cannot be empty");

            RuleForEach(x => x.WorkingHours).ChildRules(workingHour =>
            {
                workingHour.RuleFor(x => x.Day)
                    .IsInEnum()
                    .WithMessage("Invalid day of week");

                workingHour.When(x => !x.IsClosed, () =>
                {
                    workingHour.RuleFor(x => x.OpenTime)
                        .NotNull()
                        .WithMessage("Open time is required when branch is not closed");

                    workingHour.RuleFor(x => x.CloseTime)
                        .NotNull()
                        .WithMessage("Close time is required when branch is not closed");

                    workingHour.RuleFor(x => x)
                        .Must(x => x.OpenTime < x.CloseTime)
                        .When(x => x.OpenTime.HasValue && x.CloseTime.HasValue)
                        .WithMessage("Open time must be before close time");
                });
            });

            RuleFor(x => x.WorkingHours)
                .Must(workingHours => workingHours.Select(wh => wh.Day).Distinct().Count() == workingHours.Count)
                .WithMessage("Duplicate days are not allowed");
        }
    }
}
