using MediatR;
using DietManagementSystemSHFT.Models.ResponseModels;

namespace DietManagementSystemSHFT.API.CQRS.Queries.ClientQueries
{
    public record GetClientByIdQuery(Guid Id) : IRequest<ClientResponseModel>;
}