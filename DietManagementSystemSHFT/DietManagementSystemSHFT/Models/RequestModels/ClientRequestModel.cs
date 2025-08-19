namespace DietManagementSystemSHFT.Models.RequestModels
{
    public class ClientRequestModel
    {
        public string FullName { get; set; } = string.Empty;
        public double InitialWeight { get; set; }
        public double? CurrentWeight { get; set; }
        public Guid DietitianId { get; set; }
    }
}