using DietManagementSystemSHFT.Entities;

namespace DietManagementSystemSHFT.Models.ResponseModels
{
    public class DietitianResponseModel : BaseEntity
    {
       
        public string FullName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}