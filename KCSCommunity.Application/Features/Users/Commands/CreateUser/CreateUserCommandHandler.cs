using KCSCommunity.Abstractions.Interfaces;
using KCSCommunity.Domain.Constants;
using KCSCommunity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using KCSCommunity.Abstractions.Models.Configuration;

namespace KCSCommunity.Application.Features.Users.Commands.CreateUser;
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly PasscodeSettings _passcodeSettings;
    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context, ILogger<CreateUserCommandHandler> logger, PasscodeSettings passcodeSettings)
    { _userManager = userManager; _context = context; _logger = logger;_passcodeSettings = passcodeSettings; }

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = ApplicationUser.CreateNewUser(request.UserName, request.Email, request.RealName, request.EnglishName, request.Gender, request.DateOfBirth, request.RoleType, request.Grade, request.House, request.StaffTitle);
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var tempPassword = GenerateRandomPassword();
            var identityResult = await _userManager.CreateAsync(user, tempPassword);
            if (!identityResult.Succeeded) throw new Common.Exceptions.ValidationException(identityResult.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description)));
            
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            await _userManager.AddToRoleAsync(user, RoleConstants.User);

            OneTimePasscode passcode;
            const int maxRetryAttempts = 10;
            int attempt = 0;
            do
            {
                if (attempt++ >= maxRetryAttempts)
                {
                    _logger.LogError("Failed to generate a unique OneTimePasscode after {MaxAttempts} attempts.", maxRetryAttempts);
                    throw new InvalidOperationException("Could not generate a unique passcode.");
                }
                passcode = OneTimePasscode.Create(user.Id, _passcodeSettings.LifespanMinutes);
            } while (await _context.OneTimePasscodes.AnyAsync(p => p.Code == passcode.Code, cancellationToken));
            
            await _context.OneTimePasscodes.AddAsync(passcode, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("Successfully created user {UserId} with passcode {Passcode}.", user.Id, passcode.Code);
            return new CreateUserResponse(user.Id, passcode.Code);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user creation for {UserName}. Rolling back.", request.UserName);
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    private static string GenerateRandomPassword() { var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=[{]};:<>|./?"; var sb = new StringBuilder(); sb.Append(GetRandomChar("abcdefghijklmnopqrstuvwxyz")); sb.Append(GetRandomChar("ABCDEFGHIJKLMNOPQRSTUVWXYZ")); sb.Append(GetRandomChar("1234567890")); sb.Append(GetRandomChar("!@#$%^&*()_-+=[{]};:<>|./?")); for (int i = 4; i < 12; i++) sb.Append(GetRandomChar(chars)); return new string(sb.ToString().ToCharArray().OrderBy(c => GetRandomInt(0, int.MaxValue)).ToArray()); }
    private static char GetRandomChar(string source) => source[GetRandomInt(0, source.Length)];
    private static int GetRandomInt(int min, int max) => RandomNumberGenerator.GetInt32(min, max);
}