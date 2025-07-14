using KCSCommunity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using KCSCommunity.Application.Shared.Exceptions;

namespace KCSCommunity.Application.Auth.Features.Users.Commands.ChangePassword;
public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager; _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userIdStr = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null) throw new UnauthorizedAccessException();

        var user = await _userManager.FindByIdAsync(userIdStr);
        if (user == null) throw new UnauthorizedAccessException();

        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description)));
        }
    }
}