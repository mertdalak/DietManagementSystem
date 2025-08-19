using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.ClientProgressCommands
{
    public record UpdateClientProgressCommand(
        Guid Id,
        DateTime RecordDate, 
        double Weight, 
        double? BodyFatPercentage, 
        double? MuscleMass, 
        string? Notes, 
        Guid ClientId, 
        Guid DietitianId) : IRequest<BaseResponseModel>
    {
        public static UpdateClientProgressCommand FromRequest(Guid id, ClientProgressRequestModel request)
        {
            return new UpdateClientProgressCommand(
                id,
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