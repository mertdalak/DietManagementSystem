using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DietManagementSystemSHFT.Models.RequestModels;
using DietManagementSystem.Data.Enums;
using DietManagementSystemSHFT.API.CQRS.Commands.ClientProgressCommands;
using DietManagementSystemSHFT.API.CQRS.Queries.ClientProgressQueries;
using DietManagementSystemSHFT.API.CQRS.Queries.ClientQueries;
using DietManagementSystemSHFT.API.CQRS.Queries.DietitanQueries;
using DietManagementSystemSHFT.Exceptions;
using Serilog;

namespace DietManagementSystemSHFT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientProgressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientProgressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Dietitian")]
        public async Task<IActionResult> Create([FromBody] ClientProgressRequestModel request)
        {
            Log.Information("Creating new client progress record for client ID: {ClientId}", request.ClientId);
            
            // Check if user is admin or the dietitian responsible for this client
            var userRole = User.FindFirstValue("role");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid user authentication");
            }

            // Get the client to check permissions
            var client = await _mediator.Send(new GetClientByIdQuery(request.ClientId));
            if (client == null)
            {
                throw new NotFoundException($"Client with ID {request.ClientId} not found");
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
                    throw new ForbiddenException("You don't have permission to create progress records for this client");
                }

                // Override the dietitian ID to ensure it's correct
                request.DietitianId = dietitian.Id;
            }

            var command = CreateClientProgressCommand.FromRequest(request);
            var result = await _mediator.Send(command);

            Log.Information("Client progress record created successfully for client ID: {ClientId}", request.ClientId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Dietitian")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ClientProgressRequestModel request)
        {
            Log.Information("Updating client progress record with ID: {ProgressId} for client ID: {ClientId}", id, request.ClientId);
            
            // Check if user is admin or the dietitian responsible for this client
            var userRole = User.FindFirstValue("role");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid user authentication");
            }

            // Get the progress record to check permissions
            var progress = await _mediator.Send(new GetClientProgressByIdQuery(id));
            if (progress == null)
            {
                throw new NotFoundException($"Client progress record with ID {id} not found");
            }

            // Get the client to check permissions
            var client = await _mediator.Send(new GetClientByIdQuery(request.ClientId));
            if (client == null)
            {
                throw new NotFoundException($"Client with ID {request.ClientId} not found");
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
                    throw new ForbiddenException("You don't have permission to update progress records for this client");
                }

                // Override the dietitian ID to ensure it's correct
                request.DietitianId = dietitian.Id;
            }

            var command = UpdateClientProgressCommand.FromRequest(id, request);
            var result = await _mediator.Send(command);

            Log.Information("Client progress record updated successfully. Progress ID: {ProgressId}", id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Dietitian")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Log.Information("Deleting client progress record with ID: {ProgressId}", id);
            
            // Check if user is admin or the dietitian responsible for this client progress
            var userRole = User.FindFirstValue("role");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid user authentication");
            }

            // Get the progress record to check permissions
            var progress = await _mediator.Send(new GetClientProgressByIdQuery(id));
            if (progress == null)
            {
                throw new NotFoundException($"Client progress record with ID {id} not found");
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
                
                if (dietitian.Id != progress.DietitianId)
                {
                    throw new ForbiddenException("You don't have permission to delete this progress record");
                }
            }

            var command = new DeleteClientProgressCommand(id);
            var result = await _mediator.Send(command);

            Log.Information("Client progress record deleted successfully. Progress ID: {ProgressId}", id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Dietitian")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Log.Information("Getting client progress record with ID: {ProgressId}", id);
            
            // Check if user is admin or the dietitian responsible for this client progress
            var userRole = User.FindFirstValue("role");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid user authentication");
            }

            // Get the progress record
            var progress = await _mediator.Send(new GetClientProgressByIdQuery(id));
            if (progress == null)
            {
                throw new NotFoundException($"Client progress record with ID {id} not found");
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
                
                if (dietitian.Id != progress.DietitianId)
                {
                    throw new ForbiddenException("You don't have permission to view this progress record");
                }
            }

            return Ok(progress);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Dietitian")]
        public async Task<IActionResult> GetAll()
        {
            Log.Information("Getting all client progress records");
            
            var userRole = User.FindFirstValue("role");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid user authentication");
            }

            var progresses = await _mediator.Send(new GetAllClientProgressesQuery());

            // If the user is a dietitian, only return progress records for their clients
            if (userRole == Role.Dietitian.ToString())
            {
                // Get the dietitian's ID from their user ID
                var dietitians = await _mediator.Send(new GetAllDietitiansQuery());
                var dietitian = dietitians.FirstOrDefault(d => d.UserId == userId);
                
                if (dietitian == null)
                {
                    throw new NotFoundException("No dietitian profile found for this user");
                }

                progresses = progresses.Where(p => p.DietitianId == dietitian.Id).ToList();
            }

            return Ok(progresses);
        }

        [HttpGet("client/{clientId}")]
        [Authorize(Roles = "Admin,Dietitian")]
        public async Task<IActionResult> GetByClientId(Guid clientId)
        {
            Log.Information("Getting progress records for client with ID: {ClientId}", clientId);
            
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
                    throw new ForbiddenException("You don't have permission to view progress records for this client");
                }
            }

            var progresses = await _mediator.Send(new GetClientProgressesByClientIdQuery(clientId));
            return Ok(progresses);
        }
    }
}