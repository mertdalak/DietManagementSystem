using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Models;
using DietManagementSystemSHFT.API.CQRS.Queries.DietPlanQueries;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.DietPlanHandlers
{
    public class GetDietPlansByClientIdQueryHandler : IRequestHandler<GetDietPlansByClientIdQuery, IEnumerable<DietPlanResponseModel>>
    {
        private readonly DietManagementDbContext _dbContext;

        public GetDietPlansByClientIdQueryHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DietPlanResponseModel>> Handle(GetDietPlansByClientIdQuery request, CancellationToken cancellationToken)
        {
            var dietPlans = await _dbContext.DietPlans
                .Where(dp => dp.ClientId == request.ClientId && !dp.IsDeleted)
                .Select(dp => new DietPlanResponseModel
                {
                    Id = dp.Id,
                    Title = dp.Title,
                    StartDate = dp.StartDate,
                    EndDate = dp.EndDate,
                    InitialWeight = dp.InitialWeight,
                    ClientId = dp.ClientId,
                    DietitianId = dp.DietitianId,
                    CreatedAt = dp.CreatedAt,
                    UpdatedAt = dp.UpdatedAt,
                    IsDeleted = dp.IsDeleted,
                    DeletedAt = dp.DeletedAt
                })
                .ToListAsync(cancellationToken);

            return dietPlans;
        }
    }
}