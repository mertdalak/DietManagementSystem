using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Models;
using DietManagementSystemSHFT.API.CQRS.Queries.DietPlanQueries;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.DietPlanHandlers
{
    public class GetAllDietPlansQueryHandler : IRequestHandler<GetAllDietPlansQuery, IEnumerable<DietPlanResponseModel>>
    {
        private readonly DietManagementDbContext _dbContext;

        public GetAllDietPlansQueryHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DietPlanResponseModel>> Handle(GetAllDietPlansQuery request, CancellationToken cancellationToken)
        {
            var dietPlans = await _dbContext.DietPlans
                .Where(dp => !dp.IsDeleted)
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