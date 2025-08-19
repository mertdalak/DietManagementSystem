using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.API.CQRS.Commands.MealCommands;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.MealHandlers
{
    public class UpdateMealCommandHandler : IRequestHandler<UpdateMealCommand, BaseResponseModel>
    {
        private readonly DietManagementDbContext _dbContext;

        public UpdateMealCommandHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponseModel> Handle(UpdateMealCommand request, CancellationToken cancellationToken)
        {
            var meal = await _dbContext.Meals
                .Where(m => m.Id == request.Id && !m.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
                
            if (meal == null)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Meal not found"
                };
            }

            var dietPlan = await _dbContext.DietPlans
                .Where(dp => dp.Id == request.DietPlanId && !dp.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
                
            if (dietPlan == null)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Diet plan not found"
                };
            }

            if (request.EndTime <= request.StartTime)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "End time must be after start time"
                };
            }

            var mealDate = request.StartTime.Date;
            if (mealDate < dietPlan.StartDate.Date || mealDate > dietPlan.EndDate.Date)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Meal date must be within diet plan's date range"
                };
            }

            meal.Title = request.Title;
            meal.StartTime = request.StartTime;
            meal.EndTime = request.EndTime;
            meal.Contents = request.Contents;
            meal.DietPlanId = request.DietPlanId;
            meal.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseModel
            {
                IsSuccess = true,
                Message = "Meal updated successfully"
            };
        }
    }
}