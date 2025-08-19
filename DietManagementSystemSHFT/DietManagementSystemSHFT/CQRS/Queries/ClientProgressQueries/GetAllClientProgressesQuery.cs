using MediatR;
using DietManagementSystemSHFT.Models;

namespace DietManagementSystemSHFT.API.CQRS.Queries.ClientProgressQueries
{
    public record GetAllClientProgressesQuery : IRequest<IEnumerable<ClientProgressResponseModel>>;
}