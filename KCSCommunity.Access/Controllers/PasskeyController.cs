using Fido2NetLib;
using KCSCommunity.Abstractions.Models.Dtos;
using KCSCommunity.Application.Auth.Features.Authorization.Commands.Login;
using KCSCommunity.Application.Auth.Features.Passkey.Commands.AddDevice.Begin;
using KCSCommunity.Application.Auth.Features.Passkey.Commands.AddDevice.Complete;
using KCSCommunity.Application.Auth.Features.Passkey.Commands.DeleteMyPasskey;
using KCSCommunity.Application.Auth.Features.Passkey.Commands.Login.Begin;
using KCSCommunity.Application.Auth.Features.Passkey.Commands.Login.Complete;
using KCSCommunity.Application.Auth.Features.Passkey.Commands.PasskeyActivation.Begin;
using KCSCommunity.Application.Auth.Features.Passkey.Commands.PasskeyActivation.Complete;
using KCSCommunity.Application.Auth.Features.Passkey.Queries.GetMyPasskeys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KCSCommunity.Access.Controllers;

[Route("api/passkey")]
public class PasskeyController : ApiControllerBase
{

    [HttpPost("begin-activation-registration")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CredentialCreateOptions), StatusCodes.Status200OK)]
    public async Task<ActionResult<CredentialCreateOptions>> BeginActivationRegistration(BeginPasskeyActivationCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost("complete-activation-registration")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CompleteActivationRegistration([FromBody] AuthenticatorAttestationRawResponse attestationResponse)
    {
        await Mediator.Send(new CompletePasskeyActivationCommand(attestationResponse));
        return Ok();
    }
    
    [HttpPost("begin-login")]
    [AllowAnonymous]
    public async Task<ActionResult<AssertionOptions>> BeginLogin(BeginPasskeyLoginCommand command)
    {
        var options = await Mediator.Send(command);
        return Ok(options);
    }

    [HttpPost("complete-login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> CompleteLogin([FromBody] AuthenticatorAssertionRawResponse clientResponse)
    {
        var response = await Mediator.Send(new CompletePasskeyLoginCommand(clientResponse));
        return Ok(response);
    }
    
    [HttpGet("my-credentials")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<PasskeyDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PasskeyDto>>> GetMyCredentials()
    {
        var passkeys = await Mediator.Send(new GetMyPasskeysQuery());
        return Ok(passkeys);
    }

    [HttpDelete("my-credentials/{passkeyId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMyCredential([FromRoute] string passkeyId)
    {
        var decodedPasskeyId = passkeyId.Replace('-', '+').Replace('_', '/');
        await Mediator.Send(new DeleteMyPasskeyCommand(decodedPasskeyId));
        return NoContent();
    }
    
    [HttpPost("begin-add-device")]
    [Authorize]
    [ProducesResponseType(typeof(CredentialCreateOptions), StatusCodes.Status200OK)]
    public async Task<ActionResult<CredentialCreateOptions>> BeginAddDevice(BeginAddDeviceCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost("complete-add-device")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CompleteAddDevice([FromBody] AuthenticatorAttestationRawResponse attestationResponse)
    {
        await Mediator.Send(new CompleteAddDeviceCommand(attestationResponse));
        return Ok();
    }
}