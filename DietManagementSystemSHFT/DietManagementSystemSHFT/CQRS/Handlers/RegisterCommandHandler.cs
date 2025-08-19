using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DietManagementSystemSHFT.CQRS.Commands;
using DietManagementSystemSHFT.Entities;
using DietManagementSystemSHFT.Data;
using DietManagementSystemSHFT.Models.ResponseModels;
using DietManagementSystemSHFT.Settings;
using DietManagementSystemSHFT.Exceptions;

namespace DietManagementSystemSHFT.CQRS.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly DietManagementDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;

        public RegisterCommandHandler(
            UserManager<User> userManager,
            DietManagementDbContext dbContext,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponseModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new ConflictException($"User with email {request.Email} already exists");
            }

            // Create new user
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                Role = request.Role
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"Failed to create user: {errors}");
            }

            // Assign role based on Role enum
            await _userManager.AddToRoleAsync(user, request.Role.ToString());

            // Generate JWT token
            var accessToken = await GenerateJwtToken(user);
            var refreshToken = await GenerateRefreshToken(user);

            return new AuthResponseModel
            {
                IsSuccess = true,
                Message = "Registration successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
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