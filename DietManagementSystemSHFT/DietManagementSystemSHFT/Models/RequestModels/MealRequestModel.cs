namespace DietManagementSystemSHFT.Models.RequestModels
{
    public class MealRequestModel
    {
        public string Title { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Contents { get; set; } = string.Empty;
        public Guid DietPlanId { get; set; }
    }
}