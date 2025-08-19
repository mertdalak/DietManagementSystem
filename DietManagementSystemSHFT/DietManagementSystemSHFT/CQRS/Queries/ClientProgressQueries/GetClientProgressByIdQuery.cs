using MediatR;
using DietManagementSystemSHFT.Models;

namespace DietManagementSystemSHFT.API.CQRS.Queries.ClientProgressQueries
{
    public record GetClientProgressByIdQuery(Guid Id) : IRequest<ClientProgressResponseModel>;
}