using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DietManagementSystemSHFT.CQRS.Commands;
using DietManagementSystemSHFT.Entities;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Models.ResponseModels;
using DietManagementSystemSHFT.Settings;
using DietManagementSystem.Data.Enums;

namespace DietManagementSystemSHFT.CQRS.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly DietManagementDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenCommandHandler(
            UserManager<User> userManager,
            DietManagementDbContext dbContext,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponseModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _dbContext.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == request.RefreshToken && !r.IsUsed && !r.IsRevoked);

            if (refreshToken == null || refreshToken.ExpiryDate < DateTime.UtcNow)
            {
                return new AuthResponseModel
                {
                    IsSuccess = false,
                    Message = "Invalid or expired refresh token"
                };
            }

            var user = refreshToken.User;
            if (user == null)
            {
                return new AuthResponseModel
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            // Mark the current refresh token as used
            refreshToken.IsUsed = true;
            _dbContext.RefreshTokens.Update(refreshToken);

            // Generate new tokens
            var accessToken = await GenerateJwtToken(user);
            var newRefreshToken = await GenerateRefreshToken(user);

            await _dbContext.SaveChangesAsync();

            return new AuthResponseModel
            {
                IsSuccess = true,
                Message = "Token refresh successful",
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
            };
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("role", user.Role.ToString()),
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> GenerateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                UserId = user.Id
            };

            _dbContext.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync();

            return refreshToken.Token;
        }
    }
}