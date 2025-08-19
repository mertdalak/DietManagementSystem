using FluentValidation;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.Validators
{
    public class ClientRequestModelValidator : AbstractValidator<ClientRequestModel>
    {
        public ClientRequestModelValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");

            RuleFor(x => x.InitialWeight)
                .GreaterThan(0).WithMessage("Initial weight must be greater than 0.");

            RuleFor(x => x.CurrentWeight)
                .GreaterThan(0).WithMessage("Current weight must be greater than 0.")
                .When(x => x.CurrentWeight.HasValue);

            RuleFor(x => x.DietitianId)
                .NotEmpty().WithMessage("Dietitian ID is required.");
        }
    }
}