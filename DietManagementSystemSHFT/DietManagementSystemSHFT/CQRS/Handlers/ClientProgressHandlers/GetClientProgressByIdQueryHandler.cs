using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Models;
using DietManagementSystemSHFT.API.CQRS.Queries.ClientProgressQueries;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.ClientProgressHandlers
{
    public class GetClientProgressByIdQueryHandler : IRequestHandler<GetClientProgressByIdQuery, ClientProgressResponseModel>
    {
        private readonly DietManagementDbContext _dbContext;

        public GetClientProgressByIdQueryHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ClientProgressResponseModel> Handle(GetClientProgressByIdQuery request, CancellationToken cancellationToken)
        {
            var progress = await _dbContext.ClientProgressRecords
                .Where(cp => cp.Id == request.Id && !cp.IsDeleted)
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
                .FirstOrDefaultAsync(cancellationToken);

            return progress;
        }
    }
}