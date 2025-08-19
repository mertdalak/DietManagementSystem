using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DietManagementSystemSHFT.CQRS.Commands;
using DietManagementSystemSHFT.Models.RequestModels;
using DietManagementSystemSHFT.Exceptions;
using Serilog;

namespace DietManagementSystemSHFT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel request)
        {
            _logger.LogInformation("User registration attempt for: {Email}", request.Email);
            Log.Information("User registration attempt for email: {Email}", request.Email);
            
            var command = RegisterCommand.FromRequest(request);
            var result = await _mediator.Send(command);

            _logger.LogInformation("User registration successful for: {Email}", request.Email);
            Log.Information("User registration successful for email: {Email}", request.Email);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
        {
            _logger.LogInformation("User login attempt for: {Email}", request.Email);
            Log.Information("User login attempt for email: {Email}", request.Email);
            
            var command = LoginCommand.FromRequest(request);
            var result = await _mediator.Send(command);

            _logger.LogInformation("User login successful for: {Email}", request.Email);
            Log.Information("User login successful for email: {Email}", request.Email);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel request)
        {
            _logger.LogInformation("Token refresh attempt");
            Log.Information("Token refresh attempt received");
            
            var command = RefreshTokenCommand.FromRequest(request);
            var result = await _mediator.Send(command);

            _logger.LogInformation("Token refresh successful");
            Log.Information("Token refresh completed successfully");
            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var parsedUserId))
            {
                throw new UnauthorizedException("Invalid user authentication");
            }

            _logger.LogInformation("User logout for user ID: {UserId}", parsedUserId);
            Log.Information("User logout requested for user ID: {UserId}", parsedUserId);
            
            var command = new LogoutCommand(parsedUserId);
            var result = await _mediator.Send(command);

            Log.Information("User logout completed for user ID: {UserId}", parsedUserId);
            return Ok(new { Success = result });
        }
    }
}