using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Models;
using DietManagementSystemSHFT.API.CQRS.Queries.DietPlanQueries;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.DietPlanHandlers
{
    public class GetDietPlanByIdQueryHandler : IRequestHandler<GetDietPlanByIdQuery, DietPlanResponseModel>
    {
        private readonly DietManagementDbContext _dbContext;

        public GetDietPlanByIdQueryHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DietPlanResponseModel> Handle(GetDietPlanByIdQuery request, CancellationToken cancellationToken)
        {
            var dietPlan = await _dbContext.DietPlans
                .Where(dp => dp.Id == request.Id && !dp.IsDeleted)
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
                .FirstOrDefaultAsync(cancellationToken);

            return dietPlan;
        }
    }
}