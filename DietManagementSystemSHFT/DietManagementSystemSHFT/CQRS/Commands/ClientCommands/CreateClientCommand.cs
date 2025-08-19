using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.ClientCommands
{
    public record CreateClientCommand(string FullName, double InitialWeight, double? CurrentWeight, Guid DietitianId) : IRequest<BaseResponseModel>
    {
        public static CreateClientCommand FromRequest(ClientRequestModel request)
        {
            return new CreateClientCommand(request.FullName, request.InitialWeight, request.CurrentWeight, request.DietitianId);
        }
    }
}