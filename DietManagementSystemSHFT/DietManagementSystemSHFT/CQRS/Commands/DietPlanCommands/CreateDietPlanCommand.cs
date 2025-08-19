using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.DietPlanCommands
{
    public record CreateDietPlanCommand(string Title, DateTime StartDate, DateTime EndDate, double InitialWeight, Guid ClientId, Guid DietitianId) : IRequest<BaseResponseModel>
    {
        public static CreateDietPlanCommand FromRequest(DietPlanRequestModel request)
        {
            return new CreateDietPlanCommand(
                request.Title, 
                request.StartDate, 
                request.EndDate, 
                request.InitialWeight, 
                request.ClientId, 
                request.DietitianId
            );
        }
    }
}