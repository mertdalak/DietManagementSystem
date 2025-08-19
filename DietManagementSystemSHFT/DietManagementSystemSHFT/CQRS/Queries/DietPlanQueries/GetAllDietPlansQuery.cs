using MediatR;
using DietManagementSystemSHFT.Models;

namespace DietManagementSystemSHFT.API.CQRS.Queries.DietPlanQueries
{
    public record GetAllDietPlansQuery : IRequest<IEnumerable<DietPlanResponseModel>>;
}