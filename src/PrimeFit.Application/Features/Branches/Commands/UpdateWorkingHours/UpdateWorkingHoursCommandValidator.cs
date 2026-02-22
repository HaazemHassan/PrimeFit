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
                 .GreaterThan(0).WithMessage("Branch id is not valid");

            RuleFor(x => x.WorkingHours)
                .NotEmpty().WithMessage("Working hours list cannot be empty.")
                .Must(list => list.Select(x => x.Day).Distinct().Count() == list.Count)
                    .WithMessage("Duplicate days are not allowed.");

            RuleForEach(x => x.WorkingHours)
                .SetValidator(new WorkingHourDtoValidator());
        }
    }

    public class WorkingHourDtoValidator : AbstractValidator<WorkingHourDto>
    {
        public WorkingHourDtoValidator()
        {

            // If open, times are required
            When(x => !x.IsClosed, () =>
            {
                RuleFor(x => x.OpenTime)
                    .NotNull().WithMessage(x => $"{x.Day}: OpenTime is required.");

                RuleFor(x => x.CloseTime)
                    .NotNull().WithMessage(x => $"{x.Day}: CloseTime is required.");

            });
        }


    }
}
