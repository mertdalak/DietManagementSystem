namespace DietManagementSystemSHFT.Entities
{
    public class Client : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public double InitialWeight { get; set; }
        public double? CurrentWeight { get; set; }

        public Guid DietitianId { get; set; }
        public Dietitian? Dietitian { get; set; }

        public ICollection<DietPlan>? DietPlans { get; set; }
        public ICollection<ClientProgress>? ProgressRecords { get; set; }
    }
}
