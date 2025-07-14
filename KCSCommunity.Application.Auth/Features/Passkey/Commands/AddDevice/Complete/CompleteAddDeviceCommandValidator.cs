using FluentValidation;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.AddDevice.Complete;

public class CompleteAddDeviceCommandValidator : AbstractValidator<CompleteAddDeviceCommand>
{
    public CompleteAddDeviceCommandValidator()
    {
        RuleFor(x => x.AttestationResponse).NotEmpty();
    }
}