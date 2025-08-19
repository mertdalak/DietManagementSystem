using MediatR;
using DietManagementSystemSHFT.Models.ResponseModels;
using DietManagementSystemSHFT.Models.RequestModels;

namespace DietManagementSystemSHFT.CQRS.Commands
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponseModel>
    {
        public static RefreshTokenCommand FromRequest(RefreshTokenRequestModel request)
        {
            return new RefreshTokenCommand(request.RefreshToken);
        }
    }
}