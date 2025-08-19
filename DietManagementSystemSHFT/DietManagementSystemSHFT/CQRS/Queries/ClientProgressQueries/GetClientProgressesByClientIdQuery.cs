using MediatR;
using DietManagementSystemSHFT.Models;

namespace DietManagementSystemSHFT.API.CQRS.Queries.ClientProgressQueries
{
    public record GetClientProgressesByClientIdQuery(Guid ClientId) : IRequest<IEnumerable<ClientProgressResponseModel>>;
}