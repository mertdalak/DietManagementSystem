namespace DietManagementSystemSHFT.Entities
{
    public class DietPlan : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double InitialWeight { get; set; }

        public Guid ClientId { get; set; }
        public Client? Client { get; set; }

        public Guid DietitianId { get; set; }
        public Dietitian? Dietitian { get; set; }

        public ICollection<Meal>? Meals { get; set; }
    }
}
