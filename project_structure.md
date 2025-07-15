# 项目目录结构（已过滤无关目录）
└──DipontCommunity/
    ├──.github/
    │   └──workflows/
    ├──.idea/
    │   ├──.idea.DipontCommunity/
    │   │   └──.idea/
    │   └──.idea.KCSCommunity/
    │       └──.idea/
    │           ├──dataSources/
    │           └──httpRequests/
    ├──KCSCommunity.Abstractions/
    │   ├──bin/
    │   │   └──Debug/
    │   │       └──net8.0/
    │   ├──Interfaces/
    │   │   ├──IApplicationDbContext.cs
    │   │   ├──IAuthTokenSettings.cs
    │   │   └──ISessionStore.cs
    │   │   ├──Services/
    │   │   │   ├──IJwtService.cs
    │   │   │   ├──IPasskeyService.cs
    │   │   │   └──IResourceLockService.cs
    │   │   └──Validators/
    │   │       ├──IPasskeyOptionsValidator.cs
    │   │       └──IUserStatusValidator.cs
    │   └──Models/
    │       ├──Configuration/
    │       │   ├──JwtSettings.cs
    │       │   ├──PasscodeSettings.cs
    │       │   └──PasswordPolicySettings.cs
    │       └──Dtos/
    │           ├──Fido2SessionDto.cs
    │           ├──PasskeyDto.cs
    │           ├──RoleDto.cs
    │           └──UserDto.cs
    ├──KCSCommunity.Access/
    │   └──Program.cs
    │   ├──bin/
    │   │   └──Debug/
    │   │       └──net8.0/
    │   │           ├──cs/
    │   │           ├──de/
    │   │           ├──en-US/
    │   │           ├──es/
    │   │           ├──fr/
    │   │           ├──it/
    │   │           ├──ja/
    │   │           ├──ko/
    │   │           ├──pl/
    │   │           ├──pt-BR/
    │   │           ├──ru/
    │   │           ├──runtimes/
    │   │           │   ├──browser/
    │   │           │   │   └──lib/
    │   │           │   │       └──net8.0/
    │   │           │   ├──linux-arm/
    │   │           │   │   └──native/
    │   │           │   ├──linux-arm64/
    │   │           │   │   └──native/
    │   │           │   ├──linux-musl-x64/
    │   │           │   │   └──native/
    │   │           │   ├──linux-x64/
    │   │           │   │   └──native/
    │   │           │   ├──osx-arm64/
    │   │           │   │   └──native/
    │   │           │   ├──osx-x64/
    │   │           │   │   └──native/
    │   │           │   ├──win/
    │   │           │   │   └──lib/
    │   │           │   │       └──net8.0/
    │   │           │   ├──win-x64/
    │   │           │   │   └──native/
    │   │           │   └──win-x86/
    │   │           │       └──native/
    │   │           ├──tr/
    │   │           ├──zh-CN/
    │   │           ├──zh-Hans/
    │   │           ├──zh-Hant/
    │   │           └──zh-TW/
    │   ├──Controllers/
    │   │   ├──ApiControllerBase.cs
    │   │   ├──AuthController.cs
    │   │   ├──PasskeyController.cs
    │   │   ├──RolesController.cs
    │   │   └──UsersController.cs
    │   ├──Extensions/
    │   │   └──SessionExtensions.cs
    │   ├──Filters/
    │   │   └──ApiExceptionFilterAttribute.cs
    │   ├──Middleware/
    │   │   └──ApiSignatureVerificationMiddleware.cs
    │   ├──Properties/
    │   ├──Security/
    │   │   └──ApiSignature/
    │   │       └──ApiSignatureSettings.cs
    │   └──wwwroot/
    ├──KCSCommunity.Application/
    │   └──DependencyInjection.cs
    │   ├──bin/
    │   │   └──Debug/
    │   │       └──net8.0/
    │   │           ├──en-US/
    │   │           ├──zh-CN/
    │   │           └──zh-TW/
    │   └──Features/
    │       ├──Roles/
    │       │   ├──Commands/
    │       │   │   ├──CreateRole/
    │       │   │   │   ├──CreateRoleCommand.cs
    │       │   │   │   ├──CreateRoleCommandHandler.cs
    │       │   │   │   └──CreateRoleCommandValidator.cs
    │       │   │   ├──DeleteRole/
    │       │   │   │   ├──DeleteRoleCommand.cs
    │       │   │   │   ├──DeleteRoleCommandHandler.cs
    │       │   │   │   └──DeleteRoleCommandValidator.cs
    │       │   │   └──UpdateUsersInRole/
    │       │   │       ├──UpdateUsersInRoleCommand.cs
    │       │   │       ├──UpdateUsersInRoleCommandHandler.cs
    │       │   │       └──UpdateUsersInRoleCommandValidator.cs
    │       │   └──Queries/
    │       │       └──GetAllRoles/
    │       │           ├──GetAllRolesQuery.cs
    │       │           └──GetAllRolesQueryHandler.cs
    │       └──Users/
    │           ├──Commands/
    │           │   ├──ActivateUser/
    │           │   │   ├──ActivateUserCommand.cs
    │           │   │   ├──ActivateUserCommandHandler.cs
    │           │   │   └──ActivateUserCommandValidator.cs
    │           │   ├──CreateUser/
    │           │   │   ├──CreateUserCommand.cs
    │           │   │   ├──CreateUserCommandHandler.cs
    │           │   │   ├──CreateUserCommandValidator.cs
    │           │   │   └──CreateUserResponse.cs
    │           │   └──DeleteUserPasskey/
    │           │       ├──DeleteUserPasskeyCommand.cs
    │           │       ├──DeleteUserPasskeyCommandHandler.cs
    │           │       └──DeleteUserPasskeyCommandValidator.cs
    │           └──Queries/
    │               ├──GetUser/
    │               │   ├──GetUserByIdQuery.cs
    │               │   ├──GetUserByIdQueryHandler.cs
    │               │   └──GetUserByIdQueryValidator.cs
    │               └──GetUserPasskeys/
    │                   ├──GetUserPasskeysQuery.cs
    │                   ├──GetUserPasskeysQueryHandler.cs
    │                   └──GetUserPasskeysQueryValidator.cs
    ├──KCSCommunity.Application.Auth/
    │   └──DependencyInjection.cs
    │   ├──bin/
    │   │   └──Debug/
    │   │       └──net8.0/
    │   │           ├──en-US/
    │   │           ├──zh-CN/
    │   │           └──zh-TW/
    │   └──Features/
    │       ├──Authorization/
    │       │   └──Commands/
    │       │       ├──Login/
    │       │       │   ├──LoginCommand.cs
    │       │       │   ├──LoginCommandHandler.cs
    │       │       │   ├──LoginCommandValidator.cs
    │       │       │   └──LoginResponse.cs
    │       │       ├──RefreshToken/
    │       │       │   ├──RefreshTokenCommand.cs
    │       │       │   ├──RefreshTokenCommandHandler.cs
    │       │       │   └──RefreshTokenCommandValidator.cs
    │       │       └──RevokeToken/
    │       │           ├──RevokeTokenCommand.cs
    │       │           ├──RevokeTokenCommandHandler.cs
    │       │           └──RevokeTokenCommandValidator.cs
    │       ├──Passkey/
    │       │   ├──Commands/
    │       │   │   ├──AddDevice/
    │       │   │   │   ├──Begin/
    │       │   │   │   │   ├──BeginAddDeviceCommand.cs
    │       │   │   │   │   ├──BeginAddDeviceCommandHandler.cs
    │       │   │   │   │   └──BeginAddDeviceCommandValidator.cs
    │       │   │   │   └──Complete/
    │       │   │   │       ├──CompleteAddDeviceCommand.cs
    │       │   │   │       ├──CompleteAddDeviceCommandHandler.cs
    │       │   │   │       └──CompleteAddDeviceCommandValidator.cs
    │       │   │   ├──DeleteMyPasskey/
    │       │   │   │   ├──DeleteMyPasskeyCommand.cs
    │       │   │   │   ├──DeleteMyPasskeyCommandHandler.cs
    │       │   │   │   └──DeleteMyPasskeyCommandValidator.cs
    │       │   │   ├──Login/
    │       │   │   │   ├──Begin/
    │       │   │   │   │   ├──BeginPasskeyLoginCommand.cs
    │       │   │   │   │   └──BeginPasskeyLoginCommandHandler.cs
    │       │   │   │   └──Complete/
    │       │   │   │       ├──CompletePasskeyLoginCommand.cs
    │       │   │   │       ├──CompletePasskeyLoginCommandHandler.cs
    │       │   │   │       └──CompletePasskeyLoginCommandValidator.cs
    │       │   │   └──PasskeyActivation/
    │       │   │       ├──Begin/
    │       │   │       │   ├──BeginPasskeyActivationCommand.cs
    │       │   │       │   ├──BeginPasskeyActivationCommandHandler.cs
    │       │   │       │   └──BeginPasskeyActivationCommandValidator.cs
    │       │   │       └──Complete/
    │       │   │           ├──CompletePasskeyActivationCommand.cs
    │       │   │           ├──CompletePasskeyActivationCommandHandler.cs
    │       │   │           └──CompletePasskeyActivationCommandValidator.cs
    │       │   └──Queries/
    │       │       └──GetMyPasskeys/
    │       │           ├──GetMyPasskeysQuery.cs
    │       │           └──GetMyPasskeysQueryHandler.cs
    │       └──Users/
    │           └──Commands/
    │               └──ChangePassword/
    │                   ├──ChangePasswordCommand.cs
    │                   ├──ChangePasswordCommandHandler.cs
    │                   └──ChangePasswordCommandValidator.cs
    ├──KCSCommunity.Application.Shared/
    │   ├──Behaviors/
    │   │   ├──UnhandledExceptionBehaviour.cs
    │   │   └──ValidationBehaviour.cs
    │   ├──bin/
    │   │   └──Debug/
    │   │       └──net8.0/
    │   │           ├──en-US/
    │   │           ├──zh-CN/
    │   │           └──zh-TW/
    │   ├──Exceptions/
    │   │   ├──NotFoundException.cs
    │   │   ├──UserValidationException.cs
    │   │   └──ValidationException.cs
    │   ├──Mappings/
    │   │   └──MappingProfile.cs
    │   ├──Resources/
    │   │   ├──SharedValidationMessages.cs
    │   │   ├──SharedValidationMessages.en-US.Designer.cs
    │   │   ├──SharedValidationMessages.zh-CN.Designer.cs
    │   │   └──SharedValidationMessages.zh-TW.Designer.cs
    │   └──Validators/
    │       ├──PasskeyOptionsValidator.cs
    │       ├──PasswordValidators.cs
    │       └──UserStatusValidator.cs
    ├──KCSCommunity.Domain/
    │   ├──bin/
    │   │   └──Debug/
    │   │       └──net8.0/
    │   ├──Constants/
    │   │   ├──FidoConstants.cs
    │   │   ├──PolicyConstants.cs
    │   │   └──RoleConstants.cs
    │   ├──Entities/
    │   │   ├──ApplicationUser.cs
    │   │   ├──OneTimePasscode.cs
    │   │   ├──PasskeyCredential.cs
    │   │   └──RefreshToken.cs
    │   └──Enums/
    │       ├──Gender.cs
    │       └──UserRoleType.cs
    └──KCSCommunity.Infrastructure/
        └──DependencyInjection.cs
        ├──bin/
        │   └──Debug/
        │       └──net8.0/
        ├──Migrations/
        │   ├──20250704141159_InitialCreate.cs
        │   ├──20250704141159_InitialCreate.Designer.cs
        │   ├──20250705063340_RefreshTokenUpdate.cs
        │   ├──20250705063340_RefreshTokenUpdate.Designer.cs
        │   ├──20250713063446_AddPasskeyCredentials.cs
        │   ├──20250713063446_AddPasskeyCredentials.Designer.cs
        │   ├──20250714060512_RefreshUserInfo.cs
        │   ├──20250714060512_RefreshUserInfo.Designer.cs
        │   └──ApplicationDbContextModelSnapshot.cs
        ├──Persistence/
        │   ├──ApplicationDbContext.cs
        │   └──DbInitializer.cs
        │   └──Configurations/
        │       ├──ApplicationUserConfiguration.cs
        │       ├──OneTimePasscodeConfiguration.cs
        │       ├──PasskeyCredentialConfiguration.cs
        │       └──RefreshTokenConfiguration.cs
        ├──Security/
        │   ├──Hashing/
        │   │   └──Argon2PasswordHasher.cs
        │   ├──Jwt/
        │   │   └──JwtService.cs
        │   └──Passcode/
        └──Services/
            ├──Fido2PasskeyService.cs
            ├──HttpSessionStore.cs
            └──InMemoryLockService.cs

# 类与方法信息

## 文件: KCSCommunity.Application/DependencyInjection.cs
```csharp
public static class DependencyInjection
    public static IServiceCollection AddApplicationServices(this IServiceCollection services);
```

## 文件: KCSCommunity.Application.Auth/DependencyInjection.cs
```csharp
public static class DependencyInjection
    public static IServiceCollection AddApplicationAuthServices(this IServiceCollection services);
```

## 文件: KCSCommunity.Infrastructure/DependencyInjection.cs
```csharp
public static class DependencyInjection
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration);
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider);
```

## 文件: KCSCommunity.Access/Program.cs
```csharp
```

## 文件: KCSCommunity.Domain/Constants/FidoConstants.cs
```csharp
public static class FidoConstants
```

## 文件: KCSCommunity.Domain/Constants/RoleConstants.cs
```csharp
public static class RoleConstants
```

## 文件: KCSCommunity.Domain/Constants/PolicyConstants.cs
```csharp
public static class PolicyConstants
```

## 文件: KCSCommunity.Domain/Enums/Gender.cs
```csharp
```

## 文件: KCSCommunity.Domain/Enums/UserRoleType.cs
```csharp
```

## 文件: KCSCommunity.Domain/Entities/RefreshToken.cs
```csharp
public class RefreshToken
    public static string GenerateTokenValue(int numberOfBytes = 64);
```

## 文件: KCSCommunity.Domain/Entities/OneTimePasscode.cs
```csharp
public class OneTimePasscode
    private OneTimePasscode();
    public static OneTimePasscode Create(Guid userId, int lifespanMinutes = 1440); // /// Factory method for creating a new one-time passcode.   ///
    public void MarkAsUsed(); // /// Marks the passcode as used, enforcing business rules (not expired, not already used).   ///
    private static string GenerateUniqueCode(); // /// Generates a cryptographically-strong, human-readable code.   ///
```

## 文件: KCSCommunity.Domain/Entities/ApplicationUser.cs
```csharp
public class ApplicationUser // /// 拓展内置的IdentityUser<Guid>，加入新的字段 /// 这是一个Aggregate Root聚合根 /// </summary>
    private ApplicationUser();
    public static ApplicationUser CreateNewUser(string userName, string realName, string? englishName, UserRoleType roleType, string? grade, string? house, string? staffTitle); // /// Factory method for creating a new, unactivated user. This is the only public way to create a user instance.   ///
    public void ActivateAccount(); // /// Domain method to activate a user's account, enforcing business rules.   ///
    public void UpdateInformation(string realName, string? englishName, Gender gender, DateTime dateOfBirth, UserRoleType roleType, string? grade, string? house, string? staffTitle); // /// Domain method for an administrator to update a user's core information.   ///
    public void SetTimeout(DateTime endDate); // /// Domain method to set a timeout period for a user.   ///
    public void ClearTimeout(); // /// Domain method to clear a user's timeout.   ///
```

## 文件: KCSCommunity.Domain/Entities/PasskeyCredential.cs
```csharp
public class PasskeyCredential
    public Fido2NetLib.Objects.PublicKeyCredentialDescriptor GetDescriptor();
```

## 文件: KCSCommunity.Infrastructure/Migrations/20250713063446_AddPasskeyCredentials.Designer.cs
```csharp
partial class AddPasskeyCredentials
    protected override void BuildTargetModel(ModelBuilder modelBuilder);
```

## 文件: KCSCommunity.Infrastructure/Migrations/20250705063340_RefreshTokenUpdate.Designer.cs
```csharp
partial class RefreshTokenUpdate
    protected override void BuildTargetModel(ModelBuilder modelBuilder);
```

## 文件: KCSCommunity.Infrastructure/Migrations/ApplicationDbContextModelSnapshot.cs
```csharp
partial class ApplicationDbContextModelSnapshot
    protected override void BuildModel(ModelBuilder modelBuilder);
```

## 文件: KCSCommunity.Infrastructure/Migrations/20250704141159_InitialCreate.cs
```csharp
public partial class InitialCreate
    protected override void Up(MigrationBuilder migrationBuilder);
    protected override void Down(MigrationBuilder migrationBuilder);
```

## 文件: KCSCommunity.Infrastructure/Migrations/20250714060512_RefreshUserInfo.Designer.cs
```csharp
partial class RefreshUserInfo
    protected override void BuildTargetModel(ModelBuilder modelBuilder);
```

## 文件: KCSCommunity.Infrastructure/Migrations/20250704141159_InitialCreate.Designer.cs
```csharp
partial class InitialCreate
    protected override void BuildTargetModel(ModelBuilder modelBuilder);
```

## 文件: KCSCommunity.Infrastructure/Migrations/20250714060512_RefreshUserInfo.cs
```csharp
public partial class RefreshUserInfo
    protected override void Up(MigrationBuilder migrationBuilder);
    protected override void Down(MigrationBuilder migrationBuilder);
```

## 文件: KCSCommunity.Infrastructure/Migrations/20250713063446_AddPasskeyCredentials.cs
```csharp
public partial class AddPasskeyCredentials
    protected override void Up(MigrationBuilder migrationBuilder);
    protected override void Down(MigrationBuilder migrationBuilder);
```

## 文件: KCSCommunity.Infrastructure/Migrations/20250705063340_RefreshTokenUpdate.cs
```csharp
public partial class RefreshTokenUpdate
    protected override void Up(MigrationBuilder migrationBuilder);
    protected override void Down(MigrationBuilder migrationBuilder);
```

## 文件: KCSCommunity.Infrastructure/Persistence/DbInitializer.cs
```csharp
public static class DbInitializer
    public static async Task InitializeAsync(IServiceProvider serviceProvider);
    private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager, ILogger logger);
    private static async Task SeedOwnerUserAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger logger);
```

## 文件: KCSCommunity.Infrastructure/Persistence/ApplicationDbContext.cs
```csharp
public class ApplicationDbContext
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options);
    protected override void OnModelCreating(ModelBuilder builder);
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
```

## 文件: KCSCommunity.Infrastructure/Services/InMemoryLockService.cs
```csharp
public class InMemoryLockService // /// 锁，后期考虑Redis-based ///
    public InMemoryLockService(IMemoryCache cache);
    public async Task<IDisposable?> AcquireLockAsync(string resourceKey, TimeSpan timeout, CancellationToken cancellationToken = default);
private sealed class SemaphoreReleaser
    public SemaphoreReleaser(SemaphoreSlim semaphore);
    public void Dispose();
```

## 文件: KCSCommunity.Infrastructure/Services/HttpSessionStore.cs
```csharp
public class HttpSessionStore
    public HttpSessionStore(IHttpContextAccessor httpContextAccessor);
    public void Set(string key, T value);
    public T? Get(string key);
    public void Remove(string key);
```

## 文件: KCSCommunity.Infrastructure/Services/Fido2PasskeyService.cs
```csharp
public class Fido2PasskeyService
    public Fido2PasskeyService(IApplicationDbContext context, IConfiguration configuration);
    public CredentialCreateOptions GenerateRegistrationOptions(ApplicationUser user, string deviceName, List<PasskeyCredential> existingCredentials);
    public async Task<PasskeyCredential> CompleteRegistrationAsync(AuthenticatorAttestationRawResponse attestationResponse, CredentialCreateOptions originalOptions, Guid userId, string deviceName);
    public AssertionOptions GenerateLoginOptions(string? userName = null);
    public async Task<Guid> CompleteLoginAsync(AuthenticatorAssertionRawResponse clientResponse, AssertionOptions originalOptions);
```

## 文件: KCSCommunity.Abstractions/Interfaces/IAuthTokenSettings.cs
```csharp
public interface IAuthTokenSettings
     string GetSecret();
     string GetIssuer();
     string GetAudience();
     int GetAccessTokenExpiryMinutes();
     int GetRefreshTokenExpiryDays();
```

## 文件: KCSCommunity.Abstractions/Interfaces/ISessionStore.cs
```csharp
public interface ISessionStore
     void Set(string key, T value);
     T? Get(string key);
     void Remove(string key);
```

## 文件: KCSCommunity.Abstractions/Interfaces/IApplicationDbContext.cs
```csharp
public interface IApplicationDbContext
     Task<int> SaveChangesAsync(CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application.Shared/Resources/SharedValidationMessages.zh-TW.Designer.cs
```csharp
internal class SharedValidationMessages_zh_TW
    internal SharedValidationMessages_zh_TW();
```

## 文件: KCSCommunity.Application.Shared/Resources/SharedValidationMessages.en-US.Designer.cs
```csharp
internal class SharedValidationMessages_en_US
    internal SharedValidationMessages_en_US();
```

## 文件: KCSCommunity.Application.Shared/Resources/SharedValidationMessages.cs
```csharp
public class SharedValidationMessages
```

## 文件: KCSCommunity.Application.Shared/Resources/SharedValidationMessages.zh-CN.Designer.cs
```csharp
internal class SharedValidationMessages_zh_CN
    internal SharedValidationMessages_zh_CN();
```

## 文件: KCSCommunity.Application.Shared/Exceptions/UserValidationException.cs
```csharp
public class UserValidationException
    public UserValidationException(string message);
```

## 文件: KCSCommunity.Application.Shared/Exceptions/NotFoundException.cs
```csharp
public class NotFoundException
    public NotFoundException();
    public NotFoundException(string message);
    public NotFoundException(string message, Exception innerException);
    public NotFoundException(string name, object key);
```

## 文件: KCSCommunity.Application.Shared/Exceptions/ValidationException.cs
```csharp
public class ValidationException
    public ValidationException();
    public ValidationException(IEnumerable<ValidationFailure> failures);
```

## 文件: KCSCommunity.Application.Shared/Behaviors/ValidationBehaviour.cs
```csharp
public class ValidationBehaviour
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators);
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application.Shared/Behaviors/UnhandledExceptionBehaviour.cs
```csharp
public class UnhandledExceptionBehaviour
    public UnhandledExceptionBehaviour(ILogger<TRequest> logger);
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application.Shared/Mappings/MappingProfile.cs
```csharp
public class MappingProfile
    public MappingProfile();
```

## 文件: KCSCommunity.Application.Shared/Validators/UserStatusValidator.cs
```csharp
public class UserStatusValidator
    public UserStatusValidator(UserManager<ApplicationUser> userManager, IStringLocalizer<SharedValidationMessages> localizer);
    public async Task ValidateAsync(ApplicationUser user);
```

## 文件: KCSCommunity.Application.Shared/Validators/PasskeyOptionsValidator.cs
```csharp
public class PasskeyOptionsValidator
    public PasskeyOptionsValidator(IStringLocalizer<SharedValidationMessages> localizer);
    public void Validate(Fido2SessionData? options);
```

## 文件: KCSCommunity.Application.Shared/Validators/PasswordValidators.cs
```csharp
public static class PasswordValidators
    public static IRuleBuilderOptions<T, string> ApplyPasswordPolicy(this IRuleBuilder<T, string> ruleBuilder, PasswordPolicySettings policy);
```

## 文件: KCSCommunity.Access/Filters/ApiExceptionFilterAttribute.cs
```csharp
public class ApiExceptionFilterAttribute
    public ApiExceptionFilterAttribute();
    public override void OnException(ExceptionContext context);
    private void HandleException(ExceptionContext context);
    private void HandleValidationException(ExceptionContext context);
    private void HandleNotFoundException(ExceptionContext context);
    private void HandleUnauthorizedAccessException(ExceptionContext context);
    private void HandleInvalidOperationException(ExceptionContext context);
    private void HandleUnknownException(ExceptionContext context);
    private void HandleUserValidationException(ExceptionContext context);
    private void HandleFido2VerificationException(ExceptionContext context);
```

## 文件: KCSCommunity.Access/Middleware/ApiSignatureVerificationMiddleware.cs
```csharp
public class ApiSignatureVerificationMiddleware
    public ApiSignatureVerificationMiddleware(RequestDelegate next, IOptions<ApiSignatureSettings> settings, IMemoryCache cache, ILogger<ApiSignatureVerificationMiddleware> logger);
    public async Task InvokeAsync(HttpContext context);
    private static bool IsPublicPath(PathString path);
    private static async Task<string> GenerateServerSignature(HttpRequest request, string timestamp, string nonce, string apiSecret);
    private static Task WriteUnauthorizedResponse(HttpContext context, string message);
```

## 文件: KCSCommunity.Access/Extensions/SessionExtensions.cs
```csharp
public static class SessionExtensions
    public static void Set(this ISession session, string key, T value);
    public static T? Get(this ISession session, string key);
```

## 文件: KCSCommunity.Access/Controllers/PasskeyController.cs
```csharp
public class PasskeyController
    public async Task<ActionResult<CredentialCreateOptions>> BeginActivationRegistration(BeginPasskeyActivationCommand command);
    public async Task<IActionResult> CompleteActivationRegistration([FromBody] AuthenticatorAttestationRawResponse attestationResponse);
    public async Task<ActionResult<AssertionOptions>> BeginLogin(BeginPasskeyLoginCommand command);
    public async Task<ActionResult<LoginResponse>> CompleteLogin([FromBody] AuthenticatorAssertionRawResponse clientResponse);
    public async Task<ActionResult<IEnumerable<PasskeyDto>>> GetMyCredentials();
    public async Task<IActionResult> DeleteMyCredential([FromRoute] string passkeyId);
    public async Task<ActionResult<CredentialCreateOptions>> BeginAddDevice(BeginAddDeviceCommand command);
    public async Task<IActionResult> CompleteAddDevice([FromBody] AuthenticatorAttestationRawResponse attestationResponse);
```

## 文件: KCSCommunity.Access/Controllers/RolesController.cs
```csharp
public class RolesController
    public async Task<IActionResult> GetAllRoles();
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command);
    public async Task<IActionResult> DeleteRole(Guid id);
    public async Task<IActionResult> UpdateUsersInRole(Guid id, [FromBody] List<Guid> userIds);
```

## 文件: KCSCommunity.Access/Controllers/AuthController.cs
```csharp
public class AuthController
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand command);
    public async Task<ActionResult<LoginResponse>> Refresh(RefreshTokenCommand command);
    public async Task<IActionResult> Revoke(Guid userId);
```

## 文件: KCSCommunity.Access/Controllers/ApiControllerBase.cs
```csharp
public abstract class ApiControllerBase
```

## 文件: KCSCommunity.Access/Controllers/UsersController.cs
```csharp
public class UsersController
    public async Task<ActionResult<UserDto>> GetUserById(Guid id);
    public async Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserCommand command);
    public async Task<IActionResult> ActivateUser(ActivateUserCommand command);
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command);
    public async Task<ActionResult<IEnumerable<PasskeyDto>>> GetUserPasskeys(Guid userId);
    public async Task<IActionResult> DeleteUserPasskey(Guid userId, string passkeyId);
```

## 文件: KCSCommunity.Infrastructure/Security/Jwt/JwtService.cs
```csharp
public class JwtService
    public JwtService(IAuthTokenSettings tokenSettings);
    public string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
```

## 文件: KCSCommunity.Infrastructure/Security/Hashing/Argon2PasswordHasher.cs
```csharp
public class Argon2PasswordHasher
    public string HashPassword(ApplicationUser user, string password);
    public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword);
```

## 文件: KCSCommunity.Infrastructure/Persistence/Configurations/PasskeyCredentialConfiguration.cs
```csharp
public class PasskeyCredentialConfiguration
    public void Configure(EntityTypeBuilder<PasskeyCredential> builder);
```

## 文件: KCSCommunity.Infrastructure/Persistence/Configurations/ApplicationUserConfiguration.cs
```csharp
public class ApplicationUserConfiguration
    public void Configure(EntityTypeBuilder<ApplicationUser> builder);
```

## 文件: KCSCommunity.Infrastructure/Persistence/Configurations/RefreshTokenConfiguration.cs
```csharp
public class RefreshTokenConfiguration
    public void Configure(EntityTypeBuilder<RefreshToken> builder);
```

## 文件: KCSCommunity.Infrastructure/Persistence/Configurations/OneTimePasscodeConfiguration.cs
```csharp
public class OneTimePasscodeConfiguration
    public void Configure(EntityTypeBuilder<OneTimePasscode> builder);
```

## 文件: KCSCommunity.Abstractions/Models/Configuration/PasswordPolicySettings.cs
```csharp
public class PasswordPolicySettings
```

## 文件: KCSCommunity.Abstractions/Models/Configuration/JwtSettings.cs
```csharp
public class JwtSettings
    public string GetSecret();
    public string GetIssuer();
    public string GetAudience();
    public int GetAccessTokenExpiryMinutes();
    public int GetRefreshTokenExpiryDays();
```

## 文件: KCSCommunity.Abstractions/Models/Configuration/PasscodeSettings.cs
```csharp
public class PasscodeSettings
```

## 文件: KCSCommunity.Abstractions/Models/Dtos/UserDto.cs
```csharp
public record UserDto
    public UserDto();
```

## 文件: KCSCommunity.Abstractions/Models/Dtos/Fido2SessionDto.cs
```csharp
public class Fido2SessionData
```

## 文件: KCSCommunity.Abstractions/Models/Dtos/RoleDto.cs
```csharp
public record RoleDto
    public RoleDto();
```

## 文件: KCSCommunity.Abstractions/Models/Dtos/PasskeyDto.cs
```csharp
public record PasskeyDto
```

## 文件: KCSCommunity.Abstractions/Interfaces/Validators/IPasskeyOptionsValidator.cs
```csharp
public interface IPasskeyOptionsValidator
     void Validate(Fido2SessionData? user);
```

## 文件: KCSCommunity.Abstractions/Interfaces/Validators/IUserStatusValidator.cs
```csharp
public interface IUserStatusValidator
     Task ValidateAsync(ApplicationUser user);
```

## 文件: KCSCommunity.Abstractions/Interfaces/Services/IJwtService.cs
```csharp
public interface IJwtService
     string GenerateToken(ApplicationUser user, IEnumerable<string> roles); // /// Generates a JWT for a given user and their roles.   ///
```

## 文件: KCSCommunity.Abstractions/Interfaces/Services/IPasskeyService.cs
```csharp
public interface IPasskeyService
     CredentialCreateOptions GenerateRegistrationOptions(ApplicationUser user, string deviceName, List<PasskeyCredential> existingCredentials);
     Task<PasskeyCredential> CompleteRegistrationAsync(AuthenticatorAttestationRawResponse attestationResponse, CredentialCreateOptions originalOptions, Guid userId, string deviceName);
     AssertionOptions GenerateLoginOptions(string? userName = null);
     Task<Guid> CompleteLoginAsync(AuthenticatorAssertionRawResponse clientResponse, AssertionOptions originalOptions);
```

## 文件: KCSCommunity.Abstractions/Interfaces/Services/IResourceLockService.cs
```csharp
public interface IResourceLockService
     Task<IDisposable?> AcquireLockAsync(string resourceKey, TimeSpan timeout, CancellationToken cancellationToken = default); // /// Attempts to acquire a lock on a specified resource.   ///
```

## 文件: KCSCommunity.Access/Security/ApiSignature/ApiSignatureSettings.cs
```csharp
public class ApiSignatureSettings
public class ApiKeySecretPair
```

## 文件: KCSCommunity.Application/Features/Roles/Queries/GetAllRoles/GetAllRolesQueryHandler.cs
```csharp
public class GetAllRolesQueryHandler
    public GetAllRolesQueryHandler(RoleManager<IdentityRole<Guid>> roleManager, IMapper mapper);
    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application/Features/Roles/Queries/GetAllRoles/GetAllRolesQuery.cs
```csharp
public record GetAllRolesQuery
```

## 文件: KCSCommunity.Application/Features/Roles/Commands/CreateRole/CreateRoleCommand.cs
```csharp
public record CreateRoleCommand
```

## 文件: KCSCommunity.Application/Features/Roles/Commands/CreateRole/CreateRoleCommandHandler.cs
```csharp
public class CreateRoleCommandHandler
    public CreateRoleCommandHandler(RoleManager<IdentityRole<Guid>> roleManager, IStringLocalizer<SharedValidationMessages> localizer);
    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application/Features/Roles/Commands/CreateRole/CreateRoleCommandValidator.cs
```csharp
public class CreateRoleCommandValidator
    public CreateRoleCommandValidator();
```

## 文件: KCSCommunity.Application/Features/Roles/Commands/UpdateUsersInRole/UpdateUsersInRoleCommandValidator.cs
```csharp
public class UpdateUsersInRoleCommandValidator
    public UpdateUsersInRoleCommandValidator();
```

## 文件: KCSCommunity.Application/Features/Roles/Commands/UpdateUsersInRole/UpdateUsersInRoleCommandHandler.cs
```csharp
public class UpdateUsersInRoleCommandHandler
    public UpdateUsersInRoleCommandHandler(RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IResourceLockService lockService, ILogger<UpdateUsersInRoleCommandHandler> logger, IStringLocalizer<SharedValidationMessages> localizer);
    public async Task Handle(UpdateUsersInRoleCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application/Features/Roles/Commands/UpdateUsersInRole/UpdateUsersInRoleCommand.cs
```csharp
public record UpdateUsersInRoleCommand
```

## 文件: KCSCommunity.Application/Features/Roles/Commands/DeleteRole/DeleteRoleCommandHandler.cs
```csharp
public class DeleteRoleCommandHandler
    public DeleteRoleCommandHandler(RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager, IStringLocalizer<SharedValidationMessages> localizer);
    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application/Features/Roles/Commands/DeleteRole/DeleteRoleCommandValidator.cs
```csharp
public class DeleteRoleCommandValidator
    public DeleteRoleCommandValidator();
```

## 文件: KCSCommunity.Application/Features/Roles/Commands/DeleteRole/DeleteRoleCommand.cs
```csharp
public record DeleteRoleCommand
```

## 文件: KCSCommunity.Application/Features/Users/Queries/GetUser/GetUserByIdQueryHandler.cs
```csharp
public class GetUserByIdQueryHandler
    public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper);
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application/Features/Users/Queries/GetUser/GetUserByIdQuery.cs
```csharp
public record GetUserByIdQuery
```

## 文件: KCSCommunity.Application/Features/Users/Queries/GetUser/GetUserByIdQueryValidator.cs
```csharp
public class GetUserByIdQueryValidator
    public GetUserByIdQueryValidator();
```

## 文件: KCSCommunity.Application/Features/Users/Queries/GetUserPasskeys/GetUserPasskeysQueryValidator.cs
```csharp
public class GetUserPasskeysQueryValidator
    public GetUserPasskeysQueryValidator();
```

## 文件: KCSCommunity.Application/Features/Users/Queries/GetUserPasskeys/GetUserPasskeysQueryHandler.cs
```csharp
public class GetUserPasskeysQueryHandler
    public GetUserPasskeysQueryHandler(IApplicationDbContext context, IMapper mapper);
    public async Task<IEnumerable<PasskeyDto>> Handle(GetUserPasskeysQuery request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application/Features/Users/Queries/GetUserPasskeys/GetUserPasskeysQuery.cs
```csharp
public record GetUserPasskeysQuery
```

## 文件: KCSCommunity.Application/Features/Users/Commands/CreateUser/CreateUserCommandHandler.cs
```csharp
public class CreateUserCommandHandler
    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context, ILogger<CreateUserCommandHandler> logger, PasscodeSettings passcodeSettings, IStringLocalizer<SharedValidationMessages> localizer);
    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken);
    private static string GenerateRandomPassword();
    private static char GetRandomChar(string source);
    private static int GetRandomInt(int min, int max);
```

## 文件: KCSCommunity.Application/Features/Users/Commands/CreateUser/CreateUserCommandValidator.cs
```csharp
public class CreateUserCommandValidator
    public CreateUserCommandValidator();
```

## 文件: KCSCommunity.Application/Features/Users/Commands/CreateUser/CreateUserCommand.cs
```csharp
public record CreateUserCommand
```

## 文件: KCSCommunity.Application/Features/Users/Commands/CreateUser/CreateUserResponse.cs
```csharp
public record CreateUserResponse
```

## 文件: KCSCommunity.Application/Features/Users/Commands/DeleteUserPasskey/DeleteUserPasskeyCommand.cs
```csharp
public record DeleteUserPasskeyCommand
```

## 文件: KCSCommunity.Application/Features/Users/Commands/DeleteUserPasskey/DeleteUserPasskeyCommandValidator.cs
```csharp
public class DeleteUserPasskeyCommandValidator
    public DeleteUserPasskeyCommandValidator();
```

## 文件: KCSCommunity.Application/Features/Users/Commands/DeleteUserPasskey/DeleteUserPasskeyCommandHandler.cs
```csharp
public class DeleteUserPasskeyCommandHandler
    public DeleteUserPasskeyCommandHandler(IApplicationDbContext context);
    public async Task Handle(DeleteUserPasskeyCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application/Features/Users/Commands/ActivateUser/ActivateUserCommandValidator.cs
```csharp
public class ActivateUserCommandValidator
    public ActivateUserCommandValidator(PasswordPolicySettings passwordPolicy);
```

## 文件: KCSCommunity.Application/Features/Users/Commands/ActivateUser/ActivateUserCommandHandler.cs
```csharp
public class ActivateUserCommandHandler
    public ActivateUserCommandHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager, IResourceLockService lockService, ILogger<ActivateUserCommandHandler> logger, IStringLocalizer<SharedValidationMessages> localizer);
    public async Task Handle(ActivateUserCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application/Features/Users/Commands/ActivateUser/ActivateUserCommand.cs
```csharp
public record ActivateUserCommand
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Queries/GetMyPasskeys/GetMyPasskeysQueryHandler.cs
```csharp
public class GetMyPasskeysQueryHandler
    public GetMyPasskeysQueryHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper);
    public async Task<IEnumerable<PasskeyDto>> Handle(GetMyPasskeysQuery request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Queries/GetMyPasskeys/GetMyPasskeysQuery.cs
```csharp
public record GetMyPasskeysQuery
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/DeleteMyPasskey/DeleteMyPasskeyCommandHandler.cs
```csharp
public class DeleteMyPasskeyCommandHandler
    public DeleteMyPasskeyCommandHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor);
    public async Task Handle(DeleteMyPasskeyCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/DeleteMyPasskey/DeleteMyPasskeyCommandValidator.cs
```csharp
public class DeleteMyPasskeyCommandValidator
    public DeleteMyPasskeyCommandValidator();
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/DeleteMyPasskey/DeleteMyPasskeyCommand.cs
```csharp
public record DeleteMyPasskeyCommand
```

## 文件: KCSCommunity.Application.Auth/Features/Users/Commands/ChangePassword/ChangePasswordCommandHandler.cs
```csharp
public class ChangePasswordCommandHandler
    public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor);
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application.Auth/Features/Users/Commands/ChangePassword/ChangePasswordCommand.cs
```csharp
public record ChangePasswordCommand
```

## 文件: KCSCommunity.Application.Auth/Features/Users/Commands/ChangePassword/ChangePasswordCommandValidator.cs
```csharp
public class ChangePasswordCommandValidator
    public ChangePasswordCommandValidator(PasswordPolicySettings passwordPolicy, IStringLocalizer<SharedValidationMessages> localizer);
```

## 文件: KCSCommunity.Application.Auth/Features/Authorization/Commands/RefreshToken/RefreshTokenCommandHandler.cs
```csharp
public class RefreshTokenCommandHandler
    public RefreshTokenCommandHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager, IJwtService jwtService, IAuthTokenSettings tokenSettings, IStringLocalizer<SharedValidationMessages> localizer);
    public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken);
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    private async Task InvalidateUserTokensAsync(Guid userId);
```

## 文件: KCSCommunity.Application.Auth/Features/Authorization/Commands/RefreshToken/RefreshTokenCommand.cs
```csharp
public record RefreshTokenCommand
```

## 文件: KCSCommunity.Application.Auth/Features/Authorization/Commands/RefreshToken/RefreshTokenCommandValidator.cs
```csharp
public class RefreshTokenCommandValidator
    public RefreshTokenCommandValidator();
```

## 文件: KCSCommunity.Application.Auth/Features/Authorization/Commands/Login/LoginCommand.cs
```csharp
public record LoginCommand
```

## 文件: KCSCommunity.Application.Auth/Features/Authorization/Commands/Login/LoginResponse.cs
```csharp
public record LoginResponse
```

## 文件: KCSCommunity.Application.Auth/Features/Authorization/Commands/Login/LoginCommandHandler.cs
```csharp
public class LoginCommandHandler
    public LoginCommandHandler(UserManager<ApplicationUser> userManager, IJwtService jwtService, IPasswordHasher<ApplicationUser> passwordHasher, IApplicationDbContext context, IAuthTokenSettings tokenSettings, IStringLocalizer<SharedValidationMessages> localizer, IUserStatusValidator userStatusValidator);
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application.Auth/Features/Authorization/Commands/Login/LoginCommandValidator.cs
```csharp
public class LoginCommandValidator
    public LoginCommandValidator();
```

## 文件: KCSCommunity.Application.Auth/Features/Authorization/Commands/RevokeToken/RevokeTokenCommandValidator.cs
```csharp
public class RevokeTokenCommandValidator
    public RevokeTokenCommandValidator();
```

## 文件: KCSCommunity.Application.Auth/Features/Authorization/Commands/RevokeToken/RevokeTokenCommandHandler.cs
```csharp
public class RevokeTokenCommandHandler
    public RevokeTokenCommandHandler(IApplicationDbContext context);
    public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application.Auth/Features/Authorization/Commands/RevokeToken/RevokeTokenCommand.cs
```csharp
public record RevokeTokenCommand
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/PasskeyActivation/Complete/CompletePasskeyActivationCommandValidator.cs
```csharp
public class CompletePasskeyActivationCommandValidator
    public CompletePasskeyActivationCommandValidator();
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/PasskeyActivation/Complete/CompletePasskeyActivationCommand.cs
```csharp
public record CompletePasskeyActivationCommand
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/PasskeyActivation/Complete/CompletePasskeyActivationCommandHandler.cs
```csharp
public class CompletePasskeyActivationCommandHandler
    public CompletePasskeyActivationCommandHandler(IApplicationDbContext context, IPasskeyService passkeyService, UserManager<ApplicationUser> userManager, ISessionStore sessionStore, IStringLocalizer<SharedValidationMessages> localizer, IResourceLockService lockService, ILogger<CompletePasskeyActivationCommandHandler> logger, IPasskeyOptionsValidator passkeyOptionsValidator);
    public async Task Handle(CompletePasskeyActivationCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/PasskeyActivation/Begin/BeginPasskeyActivationCommandValidator.cs
```csharp
public class BeginPasskeyActivationCommandValidator
    public BeginPasskeyActivationCommandValidator();
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/PasskeyActivation/Begin/BeginPasskeyActivationCommandHandler.cs
```csharp
public class BeginPasskeyActivationCommandHandler
    public BeginPasskeyActivationCommandHandler(IApplicationDbContext context, IPasskeyService passkeyService, ISessionStore sessionStore, IStringLocalizer<SharedValidationMessages> localizer, UserManager<ApplicationUser> userManager);
    public async Task<CredentialCreateOptions> Handle(BeginPasskeyActivationCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/PasskeyActivation/Begin/BeginPasskeyActivationCommand.cs
```csharp
public record BeginPasskeyActivationCommand
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/AddDevice/Complete/CompleteAddDeviceCommandHandler.cs
```csharp
public class CompleteAddDeviceCommandHandler
    public CompleteAddDeviceCommandHandler(IApplicationDbContext context, IPasskeyService passkeyService, UserManager<ApplicationUser> userManager, ISessionStore sessionStore, IStringLocalizer<SharedValidationMessages> localizer, IHttpContextAccessor httpContextAccessor);
    public async Task Handle(CompleteAddDeviceCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/AddDevice/Complete/CompleteAddDeviceCommandValidator.cs
```csharp
public class CompleteAddDeviceCommandValidator
    public CompleteAddDeviceCommandValidator();
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/AddDevice/Complete/CompleteAddDeviceCommand.cs
```csharp
public record CompleteAddDeviceCommand
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/AddDevice/Begin/BeginAddDeviceCommandValidator.cs
```csharp
public class BeginAddDeviceCommandValidator
    public BeginAddDeviceCommandValidator();
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/AddDevice/Begin/BeginAddDeviceCommand.cs
```csharp
public record BeginAddDeviceCommand
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/AddDevice/Begin/BeginAddDeviceCommandHandler.cs
```csharp
public class BeginAddDeviceCommandHandler
    public BeginAddDeviceCommandHandler(IApplicationDbContext context, IPasskeyService passkeyService, ISessionStore sessionStore, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor);
    public async Task<CredentialCreateOptions> Handle(BeginAddDeviceCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/Login/Complete/CompletePasskeyLoginCommandHandler.cs
```csharp
public class CompletePasskeyLoginCommandHandler
    public CompletePasskeyLoginCommandHandler(IApplicationDbContext context, IPasskeyService passkeyService, UserManager<ApplicationUser> userManager, IJwtService jwtService, IAuthTokenSettings tokenSettings, ISessionStore sessionStore, IStringLocalizer<SharedValidationMessages> localizer, IUserStatusValidator userStatusValidator);
    public async Task<LoginResponse> Handle(CompletePasskeyLoginCommand request, CancellationToken cancellationToken);
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/Login/Complete/CompletePasskeyLoginCommand.cs
```csharp
public record CompletePasskeyLoginCommand
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/Login/Complete/CompletePasskeyLoginCommandValidator.cs
```csharp
public class CompletePasskeyLoginCommandValidator
    public CompletePasskeyLoginCommandValidator();
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/Login/Begin/BeginPasskeyLoginCommand.cs
```csharp
public record BeginPasskeyLoginCommand
```

## 文件: KCSCommunity.Application.Auth/Features/Passkey/Commands/Login/Begin/BeginPasskeyLoginCommandHandler.cs
```csharp
public class BeginPasskeyLoginCommandHandler
    public BeginPasskeyLoginCommandHandler(IApplicationDbContext context, IPasskeyService passkeyService, ISessionStore sessionStore);
    public Task<AssertionOptions> Handle(BeginPasskeyLoginCommand request, CancellationToken cancellationToken);
```

