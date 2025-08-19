using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.ClientCommands
{
    public record DeleteClientCommand(Guid Id) : IRequest<BaseResponseModel>;
}