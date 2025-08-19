using DietManagementSystem.Data.Enums;

namespace DietManagementSystemSHFT.Models.RequestModels
{
    public class RegisterRequestModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.Dietitian;
    }
}