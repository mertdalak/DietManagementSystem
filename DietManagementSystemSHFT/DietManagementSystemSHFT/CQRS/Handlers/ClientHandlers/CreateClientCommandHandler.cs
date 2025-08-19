using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Entities;
using DietManagementSystemSHFT.API.Models.ResponseModels;
using DietManagementSystemSHFT.API.CQRS.Commands.ClientCommands;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.ClientHandlers
{
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, BaseResponseModel>
    {
        private readonly DietManagementDbContext _dbContext;

        public CreateClientCommandHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponseModel> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            // Check if dietitian exists
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

            var client = new Client
            {
                FullName = request.FullName,
                InitialWeight = request.InitialWeight,
                CurrentWeight = request.CurrentWeight,
                DietitianId = request.DietitianId,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Clients.AddAsync(client, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseModel
            {
                IsSuccess = true,
                Message = "Client created successfully"
            };
        }
    }
}