using MediatR;
using DietManagementSystemSHFT.Models;

namespace DietManagementSystemSHFT.API.CQRS.Queries.MealQueries
{
    public record GetAllMealsQuery : IRequest<IEnumerable<MealResponseModel>>;
}