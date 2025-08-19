using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DietManagementSystemSHFT.Models.RequestModels;
using DietManagementSystem.Data.Enums;
using DietManagementSystemSHFT.API.CQRS.Commands.DietPlanCommands;
using DietManagementSystemSHFT.API.CQRS.Queries.DietPlanQueries;
using DietManagementSystemSHFT.API.CQRS.Queries.DietitanQueries;
using DietManagementSystemSHFT.API.CQRS.Queries.ClientQueries;
using DietManagementSystemSHFT.Exceptions;
using Serilog;

namespace DietManagementSystemSHFT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DietPlanController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DietPlanController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Dietitian")]
        public async Task<IActionResult> Create([FromBody] DietPlanRequestModel request)
        {
            Log.Information("Creating new diet plan for client ID: {ClientId}", request.ClientId);
            
            // If dietitian is creating a diet plan, use their ID
            if (User.FindFirstValue("role") == Role.Dietitian.ToString())
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    throw new UnauthorizedException("Invalid user authentication");
                }

                // Get the dietitian's ID from their user ID
                var dietitians = await _mediator.Send(new GetAllDietitiansQuery());
                var dietitian = dietitians.FirstOrDefault(d => d.UserId == userId);
                
                if (dietitian == null)
                {
                    throw new NotFoundException("No dietitian profile found for this user");
                }

                // Override the dietitian ID in the request
                request.DietitianId = dietitian.Id;

                // Validate that the client belongs to this dietitian
                var client = await _mediator.Send(new GetClientByIdQuery(request.ClientId));
                if (client == null)
                {
                    throw new NotFoundException($"Client with ID {request.ClientId} not found");
                }

                if (client.DietitianId != dietitian.Id)
                {
                    throw new ForbiddenException("You don't have permission to create a diet plan for this client");
                }
            }

            var command = CreateDietPlanCommand.FromRequest(request);
            var result = await _mediator.Send(command);

            Log.Information("Diet plan created successfully for client ID: {ClientId}", request.ClientId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Dietitian")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DietPlanRequestModel request)
        {
            Log.Information("Updating diet plan with ID: {DietPlanId} for client ID: {ClientId}", id, request.ClientId);
            
            // Check if user is admin or the dietitian responsible for this diet plan
            var userRole = User.FindFirstValue("role");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid user authentication");
            }

            // Get the diet plan to check permissions
            var dietPlanToCheck = await _mediator.Send(new GetDietPlanByIdQuery(id));
            if (dietPlanToCheck == null)
            {
                throw new NotFoundException($"Diet plan with ID {id} not found");
            }

            if (userRole == Role.Dietitian.ToString())
            {
                // Get the dietitian's ID from their user ID
                var dietitians = await _mediator.Send(new GetAllDietitiansQuery());
                var dietitian = dietitians.FirstOrDefault(d => d.UserId == userId);
                
                if (dietitian == null)
                {
                    throw new NotFoundException("No dietitian profile found for this user");
                }
                
                if (dietitian.Id != dietPlanToCheck.DietitianId)
                {
                    throw new ForbiddenException("You don't have permission to update this diet plan");
                }

                // Override the dietitian ID to ensure it doesn't change
                request.DietitianId = dietitian.Id;

                // Validate that the client belongs to this dietitian
                var client = await _mediator.Send(new GetClientByIdQuery(request.ClientId));
                if (client == null)
                {
                    throw new NotFoundException($"Client with ID {request.ClientId} not found");
                }

                if (client.DietitianId != dietitian.Id)
                {
                    throw new ForbiddenException("You don't have permission to update a diet plan for this client");
                }
            }

            var command = UpdateDietPlanCommand.FromRequest(id, request);
            var result = await _mediator.Send(command);

            Log.Information("Diet plan updated successfully. Diet plan ID: {DietPlanId}", id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Dietitian")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Log.Information("Deleting diet plan with ID: {DietPlanId}", id);
            
            // Check if user is admin or the dietitian responsible for this diet plan
            var userRole = User.FindFirstValue("role");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid user authentication");
            }

            // Get the diet plan to check permissions
            var dietPlanToCheck = await _mediator.Send(new GetDietPlanByIdQuery(id));
            if (dietPlanToCheck == null)
            {
                throw new NotFoundException($"Diet plan with ID {id} not found");
            }

            if (userRole == Role.Dietitian.ToString())
            {
                // Get the dietitian's ID from their user ID
                var dietitians = await _mediator.Send(new GetAllDietitiansQuery());
                var dietitian = dietitians.FirstOrDefault(d => d.UserId == userId);
                
                if (dietitian == null)
                {
                    throw new NotFoundException("No dietitian profile found for this user");
                }
                
                if (dietitian.Id != dietPlanToCheck.DietitianId)
                {
                    throw new ForbiddenException("You don't have permission to delete this diet plan");
                }
            }

            var command = new DeleteDietPlanCommand(id);
            var result = await _mediator.Send(command);

            Log.Information("Diet plan deleted successfully. Diet plan ID: {DietPlanId}", id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Dietitian")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Log.Information("Getting diet plan with ID: {DietPlanId}", id);
            
            // Check if user is admin or the dietitian responsible for this diet plan
            var userRole = User.FindFirstValue("role");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid user authentication");
            }

            // Get the diet plan to check permissions
            var dietPlan = await _mediator.Send(new GetDietPlanByIdQuery(id));
            if (dietPlan == null)
            {
                throw new NotFoundException($"Diet plan with ID {id} not found");
            }

            if (userRole == Role.Dietitian.ToString())
            {
                // Get the dietitian's ID from their user ID
                var dietitians = await _mediator.Send(new GetAllDietitiansQuery());
                var dietitian = dietitians.FirstOrDefault(d => d.UserId == userId);
                
                if (dietitian == null)
                {
                    throw new NotFoundException("No dietitian profile found for this user");
                }
                
                if (dietitian.Id != dietPlan.DietitianId)
                {
                    throw new ForbiddenException("You don't have permission to view this diet plan");
                }
            }

            return Ok(dietPlan);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Dietitian")]
        public async Task<IActionResult> GetAll()
        {
            Log.Information("Getting all diet plans");
            
            var userRole = User.FindFirstValue("role");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid user authentication");
            }

            var dietPlans = await _mediator.Send(new GetAllDietPlansQuery());

            // If the user is a dietitian, only return their diet plans
            if (userRole == Role.Dietitian.ToString())
            {
                // Get the dietitian's ID from their user ID
                var dietitians = await _mediator.Send(new GetAllDietitiansQuery());
                var dietitian = dietitians.FirstOrDefault(d => d.UserId == userId);
                
                if (dietitian == null)
                {
                    throw new NotFoundException("No dietitian profile found for this user");
                }

                dietPlans = dietPlans.Where(dp => dp.DietitianId == dietitian.Id).ToList();
            }

            return Ok(dietPlans);
        }

        [HttpGet("client/{clientId}")]
        [Authorize(Roles = "Admin,Dietitian")]
        public async Task<IActionResult> GetByClientId(Guid clientId)
        {
            Log.Information("Getting diet plans for client with ID: {ClientId}", clientId);
            
            // Check if user is admin or the dietitian responsible for this client
            var userRole = User.FindFirstValue("role");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid user authentication");
            }

            // Get the client to check permissions
            var client = await _mediator.Send(new GetClientByIdQuery(clientId));
            if (client == null)
            {
                throw new NotFoundException($"Client with ID {clientId} not found");
            }

            if (userRole == Role.Dietitian.ToString())
            {
                // Get the dietitian's ID from their user ID
                var dietitians = await _mediator.Send(new GetAllDietitiansQuery());
                var dietitian = dietitians.FirstOrDefault(d => d.UserId == userId);
                
                if (dietitian == null)
                {
                    throw new NotFoundException("No dietitian profile found for this user");
                }
                
                if (dietitian.Id != client.DietitianId)
                {
                    throw new ForbiddenException("You don't have permission to view diet plans for this client");
                }
            }

            var dietPlans = await _mediator.Send(new GetDietPlansByClientIdQuery(clientId));
            return Ok(dietPlans);
        }
    }
}