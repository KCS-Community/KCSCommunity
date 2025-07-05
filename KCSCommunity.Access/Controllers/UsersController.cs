using KCSCommunity.Application.Features.Users.Commands.ActivateUser;
using KCSCommunity.Application.Features.Users.Commands.ChangePassword;
using KCSCommunity.Application.Features.Users.Commands.CreateUser;
using KCSCommunity.Application.Features.Users.Queries.GetUser;
using KCSCommunity.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KCSCommunity.Access.Controllers;

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
}