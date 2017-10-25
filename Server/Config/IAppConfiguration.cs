namespace NetCoreTemplate.Config
{
    public interface IAppConfiguration
    {
        Logging Logging { get; }
        Smtp Smtp { get; }
        AuthSettings AuthSettings {get; }
    }
}