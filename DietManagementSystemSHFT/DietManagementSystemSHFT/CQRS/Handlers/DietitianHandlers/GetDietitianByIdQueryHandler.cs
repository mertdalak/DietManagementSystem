using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Models.ResponseModels;
using System.Linq;
using DietManagementSystemSHFT.API.CQRS.Queries.DietitanQueries;

namespace DietManagementSystemSHFT.API.CQRS.Handlers.DietitianHandler
{
    public class GetDietitianByIdQueryHandler : IRequestHandler<GetDietitianByIdQuery, DietitianResponseModel>
    {
        private readonly DietManagementDbContext _dbContext;

        public GetDietitianByIdQueryHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DietitianResponseModel> Handle(GetDietitianByIdQuery request, CancellationToken cancellationToken)
        {
            var dietitian = await _dbContext.Dietitians
                .Where(d => d.Id == request.Id && !d.IsDeleted)
                .Select(d => new DietitianResponseModel
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    Specialization = d.Specialization,
                    UserId = d.UserId,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                    IsDeleted = d.IsDeleted,
                    DeletedAt = d.DeletedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            return dietitian;
        }
    }
}