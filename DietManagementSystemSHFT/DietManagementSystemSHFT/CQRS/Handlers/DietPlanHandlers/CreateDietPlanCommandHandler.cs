using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Entities;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.API.CQRS.Commands.DietPlanCommands;
using Serilog;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.DietPlanHandlers
{
    public class CreateDietPlanCommandHandler : IRequestHandler<CreateDietPlanCommand, BaseResponseModel>
    {
        private readonly DietManagementDbContext _dbContext;

        public CreateDietPlanCommandHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponseModel> Handle(CreateDietPlanCommand request, CancellationToken cancellationToken)
        {
            Log.Information("Creating new diet plan. ClientId: {ClientId}, DietitianId: {DietitianId}, Title: {Title}", 
                request.ClientId, request.DietitianId, request.Title);
                
            // Check if client exists
            var client = await _dbContext.Clients
                .FindAsync(new object[] { request.ClientId }, cancellationToken);
                
            if (client == null)
            {
                Log.Warning("Failed to create diet plan: Client not found. ClientId: {ClientId}", request.ClientId);
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Client not found"
                };
            }

            // Check if dietitian exists
            var dietitian = await _dbContext.Dietitians
                .FindAsync(new object[] { request.DietitianId }, cancellationToken);
                
            if (dietitian == null)
            {
                Log.Warning("Failed to create diet plan: Dietitian not found. DietitianId: {DietitianId}", request.DietitianId);
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Dietitian not found"
                };
            }

            // Validate that the client belongs to the dietitian
            if (client.DietitianId != request.DietitianId)
            {
                Log.Warning("Failed to create diet plan: Client does not belong to dietitian. ClientId: {ClientId}, DietitianId: {DietitianId}", 
                    request.ClientId, request.DietitianId);
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Client does not belong to this dietitian"
                };
            }

            // Validate date range
            if (request.EndDate <= request.StartDate)
            {
                Log.Warning("Failed to create diet plan: Invalid date range. StartDate: {StartDate}, EndDate: {EndDate}", 
                    request.StartDate, request.EndDate);
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "End date must be after start date"
                };
            }

            var dietPlan = new DietPlan
            {
                Title = request.Title,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                InitialWeight = request.InitialWeight,
                ClientId = request.ClientId,
                DietitianId = request.DietitianId,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.DietPlans.AddAsync(dietPlan, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            Log.Information("Diet plan created successfully. DietPlanId: {DietPlanId}, Title: {Title}", dietPlan.Id, dietPlan.Title);
            
            return new BaseResponseModel
            {
                IsSuccess = true,
                Message = "Diet plan created successfully"
            };
        }
    }
}