using FluentValidation;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.Validators
{
    public class RefreshTokenRequestModelValidator : AbstractValidator<RefreshTokenRequestModel>
    {
        public RefreshTokenRequestModelValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}