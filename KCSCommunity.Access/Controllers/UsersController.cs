using KCSCommunity.Abstractions.Models.Dtos;
using KCSCommunity.Application.Auth.Features.Users.Commands.ChangePassword;
using KCSCommunity.Application.Features.Users.Commands.ActivateUser;
using KCSCommunity.Application.Features.Users.Commands.CreateUser;
using KCSCommunity.Application.Features.Users.Commands.DeleteUserPasskey;
using KCSCommunity.Application.Features.Users.Queries.GetUser;
using KCSCommunity.Application.Features.Users.Queries.GetUserPasskeys;
using KCSCommunity.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KCSCommunity.Access.Controllers;

[Authorize]
public class UsersController : ApiControllerBase
{
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PolicyConstants.AdminOrOwner)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        return Ok(await Mediator.Send(new GetUserByIdQuery(id)));
    }

    [HttpPost("create")]
    [Authorize(Policy = PolicyConstants.AdminOrOwner)]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetUserById), new { id = response.UserId }, response);
    }
    
    [HttpPost("activate")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActivateUser(ActivateUserCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [Authorize] // Any authenticated user can change their own password
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet("{userId:guid}/passkeys")]
    [Authorize(Policy = PolicyConstants.AdminOrOwner)]
    [ProducesResponseType(typeof(IEnumerable<PasskeyDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PasskeyDto>>> GetUserPasskeys(Guid userId)
    {
        var passkeys = await Mediator.Send(new GetUserPasskeysQuery(userId));
        return Ok(passkeys);
    }

    [HttpDelete("{userId:guid}/passkeys/{passkeyId}")]
    [Authorize(Policy = PolicyConstants.AdminOrOwner)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserPasskey(Guid userId, string passkeyId)
    {
        var decodedPasskeyId = passkeyId.Replace('-', '+').Replace('_', '/');
        await Mediator.Send(new DeleteUserPasskeyCommand(userId, decodedPasskeyId));
        return NoContent();
    }
}