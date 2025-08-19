using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.API.CQRS.Commands.MealCommands;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.MealHandlers
{
    public class DeleteMealCommandHandler : IRequestHandler<DeleteMealCommand, BaseResponseModel>
    {
        private readonly DietManagementDbContext _dbContext;

        public DeleteMealCommandHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponseModel> Handle(DeleteMealCommand request, CancellationToken cancellationToken)
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

            meal.IsDeleted = true;
            meal.DeletedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseModel
            {
                IsSuccess = true,
                Message = "Meal deleted successfully"
            };
        }
    }
}