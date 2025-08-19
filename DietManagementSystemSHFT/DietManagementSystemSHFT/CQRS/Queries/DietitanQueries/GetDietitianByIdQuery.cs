using MediatR;
using DietManagementSystemSHFT.Models.ResponseModels;

namespace DietManagementSystemSHFT.API.CQRS.Queries.DietitanQueries
{
    public record GetDietitianByIdQuery(Guid Id) : IRequest<DietitianResponseModel>;
}