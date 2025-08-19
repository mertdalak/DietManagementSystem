using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.MealCommands
{
    public record CreateMealCommand(
        string Title, 
        DateTime StartTime, 
        DateTime EndTime, 
        string Contents, 
        Guid DietPlanId) : IRequest<BaseResponseModel>
    {
        public static CreateMealCommand FromRequest(MealRequestModel request)
        {
            return new CreateMealCommand(
                request.Title, 
                request.StartTime, 
                request.EndTime, 
                request.Contents, 
                request.DietPlanId
            );
        }
    }
}