namespace KCSCommunity.Abstractions.Models.Configuration;
public class PasscodeSettings
{
    public const string SectionName = "PasscodeSettings";
    public int LifespanMinutes { get; set; } = 1440; //默认一天
}