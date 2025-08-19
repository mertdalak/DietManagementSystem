using FluentValidation;
using DietManagementSystemSHFT.Models.RequestModels;
using DietManagementSystem.Data.Enums;

namespace DietManagementSystemSHFT.Validators
{
    public class UserRequestModelValidator : AbstractValidator<UserRequestModel>
    {
        public UserRequestModelValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(x => x.Role)
                .Must(BeAValidRole).WithMessage("Role must be a valid role (Admin, Dietitian, Client).")
                .When(x => !string.IsNullOrEmpty(x.Role));
        }

        private bool BeAValidRole(string role)
        {
            return string.IsNullOrEmpty(role) || 
                   Enum.TryParse<Role>(role, out _);
        }
    }
}