namespace DietManagementSystemSHFT.Models.RequestModels
{
    public class UserRequestModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Role { get; set; } // Enum as string for API
    }
}