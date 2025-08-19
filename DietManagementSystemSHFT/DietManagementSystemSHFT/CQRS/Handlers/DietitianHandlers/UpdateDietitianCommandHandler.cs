using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Models.ResponseModels;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.API.CQRS.Commands.DietitianCommands;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.DietitianHandler
{
    public class UpdateDietitianCommandHandler : IRequestHandler<UpdateDietitianCommand, BaseResponseModel>
    {
        private readonly DietManagementDbContext _dbContext;

        public UpdateDietitianCommandHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponseModel> Handle(UpdateDietitianCommand request, CancellationToken cancellationToken)
        {
            var dietitian = await _dbContext.Dietitians
                .FirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted, cancellationToken);

            if (dietitian == null)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Dietitian not found"
                };
            }

            dietitian.FullName = request.FullName;
            dietitian.Specialization = request.Specialization;
            dietitian.UpdatedAt = DateTime.UtcNow;

            _dbContext.Dietitians.Update(dietitian);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseModel
            {
                IsSuccess = true,
                Message = "Dietitian updated successfully"
            };
        }
    }
}