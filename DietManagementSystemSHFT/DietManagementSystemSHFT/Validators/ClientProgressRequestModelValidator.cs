using FluentValidation;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.Validators
{
    public class ClientProgressRequestModelValidator : AbstractValidator<ClientProgressRequestModel>
    {
        public ClientProgressRequestModelValidator()
        {
            RuleFor(x => x.RecordDate)
                .NotEmpty().WithMessage("Record date is required.")
                .Must(BeAValidDate).WithMessage("Record date must be a valid date.");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Weight must be greater than 0.");

            RuleFor(x => x.BodyFatPercentage)
                .InclusiveBetween(0, 100).WithMessage("Body fat percentage must be between 0 and 100.")
                .When(x => x.BodyFatPercentage.HasValue);

            RuleFor(x => x.MuscleMass)
                .GreaterThan(0).WithMessage("Muscle mass must be greater than 0.")
                .When(x => x.MuscleMass.HasValue);

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Notes));

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