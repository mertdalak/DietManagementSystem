using MediatR;
using DietManagementSystemSHFT.Models;

namespace DietManagementSystemSHFT.API.CQRS.Queries.MealQueries
{
    public record GetMealByIdQuery(Guid Id) : IRequest<MealResponseModel>;
}