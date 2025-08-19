using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Models.ResponseModels;
using DietManagementSystemSHFT.API.CQRS.Queries.ClientQueries;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.ClientHandlers
{
    public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, List<ClientResponseModel>>
    {
        private readonly DietManagementDbContext _dbContext;

        public GetAllClientsQueryHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ClientResponseModel>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            var clients = await _dbContext.Clients
                .Where(c => !c.IsDeleted)
                .Select(c => new ClientResponseModel
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    InitialWeight = c.InitialWeight,
                    CurrentWeight = c.CurrentWeight,
                    DietitianId = c.DietitianId,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    IsDeleted = c.IsDeleted,
                    DeletedAt = c.DeletedAt
                })
                .ToListAsync(cancellationToken);

            return clients;
        }
    }
}