using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Application.Common.Exceptions;
using KCSCommunity.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using FluentValidation.Results;
using KCSCommunity.Domain.Entities;

namespace KCSCommunity.Application.Features.Roles.Commands.UpdateUsersInRole;
public class UpdateUsersInRoleCommandHandler : IRequestHandler<UpdateUsersInRoleCommand>
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResourceLockService _lockService;
    private readonly ILogger<UpdateUsersInRoleCommandHandler> _logger;

    public UpdateUsersInRoleCommandHandler(RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IResourceLockService lockService, ILogger<UpdateUsersInRoleCommandHandler> logger)
    {
        _roleManager = roleManager; _userManager = userManager; _httpContextAccessor = httpContextAccessor; _lockService = lockService; _logger = logger;
    }

    public async Task Handle(UpdateUsersInRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
        if (role == null) throw new NotFoundException("Role", request.RoleId);

        string lockKey = $"update-role-users-{request.RoleId}";
        using var roleLock = await _lockService.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(15), cancellationToken);

        if (roleLock == null)
        {
            _logger.LogWarning("Could not acquire lock for role modification: {RoleId}.", request.RoleId);
            throw new InvalidOperationException("This role is currently being modified. Please try again in a moment.");
        }

        var currentUsersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
        var currentUserIds = currentUsersInRole.Select(u => u.Id).ToList();
        var requestedUserIds = request.UserIds.Distinct().ToList();

        if (role.Name == RoleConstants.Owner)
        {
            var currentAdminId = Guid.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (!requestedUserIds.Any() || (currentUserIds.Count == 1 && !requestedUserIds.Contains(currentUserIds.Single())))
            {
                 throw new ValidationException(new[] { new ValidationFailure("UserIds", "The Owner role must have at least one user.") });
            }
        }
        
        var usersToAddIds = requestedUserIds.Except(currentUserIds).ToList();
        var usersToRemoveIds = currentUserIds.Except(requestedUserIds).ToList();

        foreach (var userId in usersToAddIds)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null) await _userManager.AddToRoleAsync(user, role.Name);
        }
        foreach (var userId in usersToRemoveIds)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null) await _userManager.RemoveFromRoleAsync(user, role.Name);
        }
    }
}