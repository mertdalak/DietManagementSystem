using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.DietPlanCommands
{
    public record UpdateDietPlanCommand(Guid Id, string Title, DateTime StartDate, DateTime EndDate, double InitialWeight, Guid ClientId, Guid DietitianId) : IRequest<BaseResponseModel>
    {
        public static UpdateDietPlanCommand FromRequest(Guid id, DietPlanRequestModel request)
        {
            return new UpdateDietPlanCommand(
                id,
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