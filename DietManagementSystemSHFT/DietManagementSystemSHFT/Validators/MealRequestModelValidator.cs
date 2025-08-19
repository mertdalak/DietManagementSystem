using FluentValidation;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.Validators
{
    public class MealRequestModelValidator : AbstractValidator<MealRequestModel>
    {
        public MealRequestModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");

            RuleFor(x => x.Contents)
                .NotEmpty().WithMessage("Contents are required.");

            RuleFor(x => x.DietPlanId)
                .NotEmpty().WithMessage("Diet plan ID is required.");
        }
    }
}