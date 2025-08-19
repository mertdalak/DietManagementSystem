using MediatR;
using DietManagementSystemSHFT.Models.ResponseModels;

namespace DietManagementSystemSHFT.CQRS.Commands
{
    public record LogoutCommand(Guid UserId) : IRequest<bool>;
}