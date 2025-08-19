using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Models;
using DietManagementSystemSHFT.API.CQRS.Queries.ClientProgressQueries;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.ClientProgressHandlers
{
    public class GetAllClientProgressesQueryHandler : IRequestHandler<GetAllClientProgressesQuery, IEnumerable<ClientProgressResponseModel>>
    {
        private readonly DietManagementDbContext _dbContext;

        public GetAllClientProgressesQueryHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ClientProgressResponseModel>> Handle(GetAllClientProgressesQuery request, CancellationToken cancellationToken)
        {
            var progresses = await _dbContext.ClientProgressRecords
                .Where(cp => !cp.IsDeleted)
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