using MediatR;
using DietManagementSystemSHFT.API.Models.ResponseModels;

namespace DietManagementSystemSHFT.API.CQRS.Commands.MealCommands
{
    public record DeleteMealCommand(Guid Id) : IRequest<BaseResponseModel>;
}