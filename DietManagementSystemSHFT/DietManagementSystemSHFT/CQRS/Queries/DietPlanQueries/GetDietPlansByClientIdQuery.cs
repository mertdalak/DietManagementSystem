using MediatR;
using DietManagementSystemSHFT.Models;

namespace DietManagementSystemSHFT.API.CQRS.Queries.DietPlanQueries
{
    public record GetDietPlansByClientIdQuery(Guid ClientId) : IRequest<IEnumerable<DietPlanResponseModel>>;
}