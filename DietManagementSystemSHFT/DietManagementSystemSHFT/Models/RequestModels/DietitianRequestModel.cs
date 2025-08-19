namespace DietManagementSystemSHFT.Models.RequestModels
{
    public class DietitianRequestModel
    {
       
        public string FullName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}