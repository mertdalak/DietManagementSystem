using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.DietitianCommands
{
    public record UpdateDietitianCommand(Guid Id, string FullName, string Specialization) : IRequest<BaseResponseModel>
    {
        public static UpdateDietitianCommand FromRequest(Guid id, DietitianRequestModel request)
        {
            return new UpdateDietitianCommand(id, request.FullName, request.Specialization);
        }
    }
}