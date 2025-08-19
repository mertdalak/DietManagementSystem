using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.ClientProgressCommands
{
    public record DeleteClientProgressCommand(Guid Id) : IRequest<BaseResponseModel>;
}