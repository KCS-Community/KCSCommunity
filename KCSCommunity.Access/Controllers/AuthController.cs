using KCSCommunity.Application.Features.Authorization.Commands.Login;
using KCSCommunity.Application.Features.Authorization.Commands.RefreshToken;
using KCSCommunity.Application.Features.Authorization.Commands.RevokeToken;
using KCSCommunity.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KCSCommunity.Access.Controllers;

[AllowAnonymous]
public class AuthController : ApiControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPost("refresh")]
    [AllowAnonymous] // Refresh endpoint is public
    public async Task<ActionResult<LoginResponse>> Refresh(RefreshTokenCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [Authorize(Policy = PolicyConstants.AdminOrOwner)]
    [HttpPost("revoke/{userId:guid}")]
    public async Task<IActionResult> Revoke(Guid userId)
    {
        await Mediator.Send(new RevokeTokenCommand(userId));
        return NoContent();
    }
}