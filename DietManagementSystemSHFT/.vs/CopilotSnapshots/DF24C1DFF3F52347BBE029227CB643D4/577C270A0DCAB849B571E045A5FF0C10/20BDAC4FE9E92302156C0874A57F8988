using Microsoft.AspNetCore.Identity;
using DietManagementSystem.Data.Enums;

namespace DietManagementSystemSHFT.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.Dietitian;

        public Dietitian? DietitianProfile { get; set; }
    }
}
