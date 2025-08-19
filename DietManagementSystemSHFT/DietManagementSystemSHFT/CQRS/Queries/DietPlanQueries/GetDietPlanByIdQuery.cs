using MediatR;
using DietManagementSystemSHFT.Models;

namespace DietManagementSystemSHFT.API.CQRS.Queries.DietPlanQueries
{
    public record GetDietPlanByIdQuery(Guid Id) : IRequest<DietPlanResponseModel>;
}