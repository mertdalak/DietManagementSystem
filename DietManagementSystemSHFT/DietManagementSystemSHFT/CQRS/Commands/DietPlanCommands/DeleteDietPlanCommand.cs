using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.DietPlanCommands
{
    public record DeleteDietPlanCommand(Guid Id) : IRequest<BaseResponseModel>;
}