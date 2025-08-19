using System.Text.Json.Serialization;

namespace DietManagementSystemSHFT.API.Models.ResponseModels
{
    public class ErrorResponseModel : BaseResponseModel
    {
        public ErrorResponseModel()
        {
            IsSuccess = false;
        }

        public ErrorResponseModel(string message, string errorCode = null, object details = null)
        {
            IsSuccess = false;
            Message = message;
            ErrorCode = errorCode;
            Details = details;
        }

        public string ErrorCode { get; set; }
        public object Details { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string StackTrace { get; set; }
    }
}