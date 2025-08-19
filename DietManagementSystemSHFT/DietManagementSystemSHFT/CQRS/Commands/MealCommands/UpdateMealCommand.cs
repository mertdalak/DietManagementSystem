using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.MealCommands
{
    public record UpdateMealCommand(
        Guid Id, 
        string Title, 
        DateTime StartTime, 
        DateTime EndTime, 
        string Contents, 
        Guid DietPlanId) : IRequest<BaseResponseModel>
    {
        public static UpdateMealCommand FromRequest(Guid id, MealRequestModel request)
        {
            return new UpdateMealCommand(
                id,
                request.Title, 
                request.StartTime, 
                request.EndTime, 
                request.Contents, 
                request.DietPlanId
            );
        }
    }
}