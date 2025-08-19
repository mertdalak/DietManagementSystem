using MediatR;
using DietManagementSystemSHFT.Models.ResponseModels;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.CQRS.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthResponseModel>
    {
        public static LoginCommand FromRequest(LoginRequestModel request)
        {
            return new LoginCommand(request.Email, request.Password);
        }
    }
}