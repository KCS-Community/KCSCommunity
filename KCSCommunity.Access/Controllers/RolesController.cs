using KCSCommunity.Application.Features.Roles.Commands.CreateRole;
using KCSCommunity.Application.Features.Roles.Commands.DeleteRole;
using KCSCommunity.Application.Features.Roles.Commands.UpdateUsersInRole;
using KCSCommunity.Application.Features.Roles.Queries.GetAllRoles;
using KCSCommunity.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KCSCommunity.Access.Controllers;

[Authorize(Policy = PolicyConstants.OwnerOnly)]
public class RolesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRoles()
    {
        return Ok(await Mediator.Send(new GetAllRolesQuery()));
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
    {
        var roleId = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetAllRoles), new { id = roleId }, new { id = roleId });
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        await Mediator.Send(new DeleteRoleCommand(id));
        return NoContent();
    }
    
    [HttpPut("{id:guid}/users")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUsersInRole(Guid id, [FromBody] List<Guid> userIds)
    {
        await Mediator.Send(new UpdateUsersInRoleCommand(id, userIds));
        return NoContent();
    }
}