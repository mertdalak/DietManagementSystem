using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Models;
using DietManagementSystemSHFT.API.CQRS.Queries.MealQueries;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.MealHandlers
{
    public class GetAllMealsQueryHandler : IRequestHandler<GetAllMealsQuery, IEnumerable<MealResponseModel>>
    {
        private readonly DietManagementDbContext _dbContext;

        public GetAllMealsQueryHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MealResponseModel>> Handle(GetAllMealsQuery request, CancellationToken cancellationToken)
        {
            var meals = await _dbContext.Meals
                .Where(m => !m.IsDeleted)
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
                .ToListAsync(cancellationToken);

            return meals;
        }
    }
}