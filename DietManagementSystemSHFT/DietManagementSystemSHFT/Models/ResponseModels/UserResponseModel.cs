using DietManagementSystemSHFT.Entities;

namespace DietManagementSystemSHFT.Models
{
    public class UserResponseModel : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Role { get; set; } // Enum as string for API
    }
}