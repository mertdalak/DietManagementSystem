using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.DietitianCommands
{
    public record DeleteDietitianCommand(Guid Id) : IRequest<BaseResponseModel>;
}