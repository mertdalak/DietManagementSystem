using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Entities;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.API.CQRS.Commands.ClientProgressCommands;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.ClientProgressHandlers
{
    public class CreateClientProgressCommandHandler : IRequestHandler<CreateClientProgressCommand, BaseResponseModel>
    {
        private readonly DietManagementDbContext _dbContext;

        public CreateClientProgressCommandHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponseModel> Handle(CreateClientProgressCommand request, CancellationToken cancellationToken)
        {
            // Check if client exists
            var client = await _dbContext.Clients
                .Where(c => c.Id == request.ClientId && !c.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
                
            if (client == null)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Client not found"
                };
            }

            // Check if dietitian exists
            var dietitian = await _dbContext.Dietitians
                .Where(d => d.Id == request.DietitianId && !d.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
                
            if (dietitian == null)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Dietitian not found"
                };
            }

            // Validate that the client belongs to the dietitian
            if (client.DietitianId != request.DietitianId)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Client does not belong to this dietitian"
                };
            }

            // Validate weight is positive
            if (request.Weight <= 0)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Weight must be a positive value"
                };
            }

            // Validate body fat percentage is between 0 and 100 if provided
            if (request.BodyFatPercentage.HasValue && (request.BodyFatPercentage < 0 || request.BodyFatPercentage > 100))
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Body fat percentage must be between 0 and 100"
                };
            }

            // Validate muscle mass is positive if provided
            if (request.MuscleMass.HasValue && request.MuscleMass <= 0)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Muscle mass must be a positive value"
                };
            }

            var clientProgress = new ClientProgress
            {
                RecordDate = request.RecordDate,
                Weight = request.Weight,
                BodyFatPercentage = request.BodyFatPercentage,
                MuscleMass = request.MuscleMass,
                Notes = request.Notes,
                ClientId = request.ClientId,
                DietitianId = request.DietitianId,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.ClientProgressRecords.AddAsync(clientProgress, cancellationToken);
            
            // Update client's current weight
            client.CurrentWeight = request.Weight;
            client.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseModel
            {
                IsSuccess = true,
                Message = "Client progress record created successfully"
            };
        }
    }
}