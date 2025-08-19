using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.ClientProgressCommands
{
    public record CreateClientProgressCommand(
        DateTime RecordDate, 
        double Weight, 
        double? BodyFatPercentage, 
        double? MuscleMass, 
        string? Notes, 
        Guid ClientId, 
        Guid DietitianId) : IRequest<BaseResponseModel>
    {
        public static CreateClientProgressCommand FromRequest(ClientProgressRequestModel request)
        {
            return new CreateClientProgressCommand(
                request.RecordDate,
                request.Weight,
                request.BodyFatPercentage,
                request.MuscleMass,
                request.Notes,
                request.ClientId,
                request.DietitianId
            );
        }
    }
}