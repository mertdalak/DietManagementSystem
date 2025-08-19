using DietManagementSystemSHFT.API.Models.ResponseModels;

namespace DietManagementSystemSHFT.Models.ResponseModels
{
    public class AuthResponseModel : BaseResponseModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? Expiration { get; set; }
    }
}