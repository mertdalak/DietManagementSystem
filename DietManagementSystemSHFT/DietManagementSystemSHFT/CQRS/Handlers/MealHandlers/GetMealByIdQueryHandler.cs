using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Models;
using DietManagementSystemSHFT.API.CQRS.Queries.MealQueries;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.MealHandlers
{
    public class GetMealByIdQueryHandler : IRequestHandler<GetMealByIdQuery, MealResponseModel>
    {
        private readonly DietManagementDbContext _dbContext;

        public GetMealByIdQueryHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MealResponseModel> Handle(GetMealByIdQuery request, CancellationToken cancellationToken)
        {
            var meal = await _dbContext.Meals
                .Where(m => m.Id == request.Id && !m.IsDeleted)
                .Select(m => new MealResponseModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    StartTime = m.StartTime,
                    EndTime = m.EndTime,
                    Contents = m.Contents,
                    DietPlanId = m.DietPlanId,
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt,
                    IsDeleted = m.IsDeleted,
                    DeletedAt = m.DeletedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            return meal;
        }
    }
}