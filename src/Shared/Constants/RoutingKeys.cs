namespace Shared.Constants;

public static class RoutingKeys
{
    public const string CriticalAlert = "alert.critical";
    public const string LoginFailed = "security.login.failed";
    public const string LoginSuccess = "security.login.success";
    public const string SystemStarted = "security.system.started";
    public const string AllSecurityEvents = "security.#";
    public const string LoginEvents = "security.login.*";
    public const string AllEvents = "#";
}
