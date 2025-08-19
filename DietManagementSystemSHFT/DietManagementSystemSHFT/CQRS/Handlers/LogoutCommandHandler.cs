using MediatR;
using Microsoft.EntityFrameworkCore;
using DietManagementSystemSHFT.CQRS.Commands;
using DietManagementSystemSHFT.Data;

namespace DietManagementSystemSHFT.CQRS.Handlers
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly DietManagementDbContext _dbContext;

        public LogoutCommandHandler(DietManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var refreshTokens = await _dbContext.RefreshTokens
                .Where(r => r.UserId == request.UserId && !r.IsUsed && !r.IsRevoked)
                .ToListAsync(cancellationToken);

            if (refreshTokens.Any())
            {
                foreach (var token in refreshTokens)
                {
                    token.IsRevoked = true;
                }

                _dbContext.RefreshTokens.UpdateRange(refreshTokens);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return true;
        }
    }
}