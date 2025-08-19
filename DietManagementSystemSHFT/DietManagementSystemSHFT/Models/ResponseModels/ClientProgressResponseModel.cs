using DietManagementSystemSHFT.Entities;

namespace DietManagementSystemSHFT.Models
{
    public class ClientProgressResponseModel : BaseEntity 
    {

        public DateTime RecordDate { get; set; }
        public double Weight { get; set; }
        public double? BodyFatPercentage { get; set; }
        public double? MuscleMass { get; set; }
        public string? Notes { get; set; }
        public Guid ClientId { get; set; }
        public Guid DietitianId { get; set; }
    }
}