using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.DietitianCommands
{
    public record CreateDietitianCommand(string FullName, string Specialization, Guid UserId) : IRequest<BaseResponseModel>
    {
        public static CreateDietitianCommand FromRequest(DietitianRequestModel request)
        {
            return new CreateDietitianCommand(request.FullName, request.Specialization, request.UserId);
        }
    }
}