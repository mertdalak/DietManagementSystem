using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.API.CQRS.Commands.DietPlanCommands;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.DietPlanHandlers
{
    public class UpdateDietPlanCommandHandler : IRequestHandler<UpdateDietPlanCommand, BaseResponseModel>
    {
        private readonly DietManagementDbContext _dbContext;

        public UpdateDietPlanCommandHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponseModel> Handle(UpdateDietPlanCommand request, CancellationToken cancellationToken)
        {
            var dietPlan = await _dbContext.DietPlans
                .Where(dp => dp.Id == request.Id && !dp.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
                
            if (dietPlan == null)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Diet plan not found"
                };
            }

            var client = await _dbContext.Clients
                .FindAsync(new object[] { request.ClientId }, cancellationToken);
                
            if (client == null)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Client not found"
                };
            }

            var dietitian = await _dbContext.Dietitians
                .FindAsync(new object[] { request.DietitianId }, cancellationToken);
                
            if (dietitian == null)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Dietitian not found"
                };
            }

            if (client.DietitianId != request.DietitianId)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Client does not belong to this dietitian"
                };
            }

            if (request.EndDate <= request.StartDate)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "End date must be after start date"
                };
            }

            dietPlan.Title = request.Title;
            dietPlan.StartDate = request.StartDate;
            dietPlan.EndDate = request.EndDate;
            dietPlan.InitialWeight = request.InitialWeight;
            dietPlan.ClientId = request.ClientId;
            dietPlan.DietitianId = request.DietitianId;
            dietPlan.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseModel
            {
                IsSuccess = true,
                Message = "Diet plan updated successfully"
            };
        }
    }
}