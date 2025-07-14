namespace KCSCommunity.Abstractions.Interfaces;

public interface IAuthTokenSettings
{
    string GetSecret();
    string GetIssuer();
    string GetAudience();
    int GetAccessTokenExpiryMinutes();
    int GetRefreshTokenExpiryDays();
}