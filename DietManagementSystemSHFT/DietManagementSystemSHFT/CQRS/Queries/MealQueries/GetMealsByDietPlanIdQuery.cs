using MediatR;
using DietManagementSystemSHFT.Models;

namespace DietManagementSystemSHFT.API.CQRS.Queries.MealQueries
{
    public record GetMealsByDietPlanIdQuery(Guid DietPlanId) : IRequest<IEnumerable<MealResponseModel>>;
}