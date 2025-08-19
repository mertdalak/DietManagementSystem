using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.ClientCommands
{
    public record UpdateClientCommand(Guid Id, string FullName, double InitialWeight, double? CurrentWeight, Guid DietitianId) : IRequest<BaseResponseModel>
    {
        public static UpdateClientCommand FromRequest(Guid id, ClientRequestModel request)
        {
            return new UpdateClientCommand(id, request.FullName, request.InitialWeight, request.CurrentWeight, request.DietitianId);
        }
    }
}