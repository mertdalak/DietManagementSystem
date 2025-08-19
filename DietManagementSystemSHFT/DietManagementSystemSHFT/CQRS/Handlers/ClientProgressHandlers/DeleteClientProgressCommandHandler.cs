using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.API.CQRS.Commands.ClientProgressCommands;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.ClientProgressHandlers
{
    public class DeleteClientProgressCommandHandler : IRequestHandler<DeleteClientProgressCommand, BaseResponseModel>
    {
        private readonly DietManagementDbContext _dbContext;

        public DeleteClientProgressCommandHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponseModel> Handle(DeleteClientProgressCommand request, CancellationToken cancellationToken)
        {
            var clientProgress = await _dbContext.ClientProgressRecords
                .Where(cp => cp.Id == request.Id && !cp.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
                
            if (clientProgress == null)
            {
                return new BaseResponseModel
                {
                    IsSuccess = false,
                    Message = "Client progress record not found"
                };
            }

            clientProgress.IsDeleted = true;
            clientProgress.DeletedAt = DateTime.UtcNow;

            var client = await _dbContext.Clients
                .Where(c => c.Id == clientProgress.ClientId)
                .FirstOrDefaultAsync(cancellationToken);

            if (client != null)
            {
                var latestProgress = await _dbContext.ClientProgressRecords
                    .Where(cp => cp.ClientId == clientProgress.ClientId && !cp.IsDeleted && cp.Id != request.Id)
                    .OrderByDescending(cp => cp.RecordDate)
                    .FirstOrDefaultAsync(cancellationToken);

                if (latestProgress != null)
                {
                    client.CurrentWeight = latestProgress.Weight;
                }
                else
                {
                    client.CurrentWeight = client.InitialWeight;
                }

                client.UpdatedAt = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseModel
            {
                IsSuccess = true,
                Message = "Client progress record deleted successfully"
            };
        }
    }
}