namespace DietManagementSystemSHFT.Models.RequestModels
{
    public class DietPlanRequestModel
    {
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double InitialWeight { get; set; }
        public Guid ClientId { get; set; }
        public Guid DietitianId { get; set; }
    }
}