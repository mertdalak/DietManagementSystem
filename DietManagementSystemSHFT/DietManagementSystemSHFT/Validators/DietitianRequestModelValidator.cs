using FluentValidation;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.Validators
{
    public class DietitianRequestModelValidator : AbstractValidator<DietitianRequestModel>
    {
        public DietitianRequestModelValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");

            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Specialization is required.")
                .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}