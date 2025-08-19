using MediatR;
using DietManagementSystemSHFT.Models.ResponseModels;
using DietManagementSystemSHFT.Models.RequestModels;
using DietManagementSystem.Data.Enums;

namespace DietManagementSystemSHFT.CQRS.Commands
{
    public record RegisterCommand(
        string FullName,
        string Email,
        string Password,
        Role Role) : IRequest<AuthResponseModel>
    {
        public static RegisterCommand FromRequest(RegisterRequestModel request)
        {
            return new RegisterCommand(
                request.FullName,
                request.Email,
                request.Password,
                request.Role);
        }
    }
}