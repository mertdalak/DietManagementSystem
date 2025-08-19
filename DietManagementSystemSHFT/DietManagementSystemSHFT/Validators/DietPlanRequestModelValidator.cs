using FluentValidation;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.Validators
{
    public class DietPlanRequestModelValidator : AbstractValidator<DietPlanRequestModel>
    {
        public DietPlanRequestModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .Must(BeAValidDate).WithMessage("Start date must be a valid date.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .Must(BeAValidDate).WithMessage("End date must be a valid date.")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

            RuleFor(x => x.InitialWeight)
                .GreaterThan(0).WithMessage("Initial weight must be greater than 0.");

            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("Client ID is required.");

            RuleFor(x => x.DietitianId)
                .NotEmpty().WithMessage("Dietitian ID is required.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return date != default && date > new DateTime(1900, 1, 1) && date < new DateTime(2100, 1, 1);
        }
    }
}