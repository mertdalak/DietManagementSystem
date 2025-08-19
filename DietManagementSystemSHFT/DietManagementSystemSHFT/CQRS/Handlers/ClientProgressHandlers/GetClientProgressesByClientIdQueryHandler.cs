using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Models;
using DietManagementSystemSHFT.API.CQRS.Queries.ClientProgressQueries;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.ClientProgressHandlers
{
    public class GetClientProgressesByClientIdQueryHandler : IRequestHandler<GetClientProgressesByClientIdQuery, IEnumerable<ClientProgressResponseModel>>
    {
        private readonly DietManagementDbContext _dbContext;

        public GetClientProgressesByClientIdQueryHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ClientProgressResponseModel>> Handle(GetClientProgressesByClientIdQuery request, CancellationToken cancellationToken)
        {
            var progresses = await _dbContext.ClientProgressRecords
                .Where(cp => cp.ClientId == request.ClientId && !cp.IsDeleted)
                .OrderByDescending(cp => cp.RecordDate)
                .Select(cp => new ClientProgressResponseModel
                {
                    Id = cp.Id,
                    RecordDate = cp.RecordDate,
                    Weight = cp.Weight,
                    BodyFatPercentage = cp.BodyFatPercentage,
                    MuscleMass = cp.MuscleMass,
                    Notes = cp.Notes,
                    ClientId = cp.ClientId,
                    DietitianId = cp.DietitianId,
                    CreatedAt = cp.CreatedAt,
                    UpdatedAt = cp.UpdatedAt,
                    IsDeleted = cp.IsDeleted,
                    DeletedAt = cp.DeletedAt
                })
                .ToListAsync(cancellationToken);

            return progresses;
        }
    }
}