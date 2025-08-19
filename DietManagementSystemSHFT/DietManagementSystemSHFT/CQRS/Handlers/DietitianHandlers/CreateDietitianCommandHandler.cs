using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Entities;
using DietManagementSystemSHFT.Models.ResponseModels;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.API.CQRS.Commands.DietitianCommands;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.DietitianHandler
{
    public class CreateDietitianCommandHandler : IRequestHandler<CreateDietitianCommand, BaseResponseModel>
    {
        private readonly DietManagementDbContext _dbContext;

        public CreateDietitianCommandHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponseModel> Handle(CreateDietitianCommand request, CancellationToken cancellationToken)
        {
            // Check if user exists
            var user = await _dbContext.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            // Check if dietitian already exists for this user
            var existingDietitian = await _dbContext.Dietitians
                .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

            if (existingDietitian != null)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "A dietitian profile already exists for this user"
                };
            }

            var dietitian = new Dietitian
            {
                FullName = request.FullName,
                Specialization = request.Specialization,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Dietitians.AddAsync(dietitian, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseModel
            {
                IsSuccess = true,
                Message = "Dietitian created successfully"
            };
        }
    }
}