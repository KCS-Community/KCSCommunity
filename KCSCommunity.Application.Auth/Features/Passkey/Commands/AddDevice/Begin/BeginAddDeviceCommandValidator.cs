using FluentValidation;

namespace KCSCommunity.Application.Auth.Features.Passkey.Commands.AddDevice.Begin;

public class BeginAddDeviceCommandValidator : AbstractValidator<BeginAddDeviceCommand>
{
    public BeginAddDeviceCommandValidator()
    {
        RuleFor(x => x.DeviceName).NotEmpty().MaximumLength(100);
    }
}