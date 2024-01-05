using SkyFrost.Base;
using SkyFrostOutputDevice = SkyFrost.Base.OutputDevice;

namespace Jworkz.ResonitePowerShellModule.SkyFrost;

using Clients.Abstract;

public class PowerShellStatusSource : IUserStatusSource
{
    public const byte ARTIFICIAL_SECONDS_DECREMENT = 5;

    public const byte MIN_SECONDS_BEFORE_UPDATE = 60;

    private string _compatibilityHash = string.Empty;

    private Task? _currentOnlineInstanceTask;

    private readonly string _productName;

    private DateTime _lastOnlineInstanceTime;

    private ISkyFrostInterfaceClient _skyFrostInterfaceClient;

    private readonly string _version;

    public bool LoadingOnlineStatus { get; private set; }

    public string CompatibilityHash => _compatibilityHash;

    public string AppVersion => $"{_productName} {_version}";

    public bool IsMobile => false;

    public UserSessionType SessionType => UserSessionType.Unknown;

    public bool IsUserPresent => true;

    public DateTime LastPresenceTimestamp => DateTime.UtcNow.AddSeconds(-ARTIFICIAL_SECONDS_DECREMENT);

    public SkyFrostOutputDevice? OutputDevice => SkyFrostOutputDevice.Screen;

    public DateTime LastSessionChangeTimestamp => DateTime.UtcNow.AddSeconds(-(ARTIFICIAL_SECONDS_DECREMENT * ARTIFICIAL_SECONDS_DECREMENT));

    public PowerShellStatusSource(ISkyFrostInterfaceClient client, string productName, string version)
    {
        _productName = productName;
        _version = version;
        _compatibilityHash = CryptoHelper.HashIDToToken(AppVersion);
        _skyFrostInterfaceClient = client;
    }

    public bool BeginUpdate()
    {
        var hasRunningTask = !_currentOnlineInstanceTask?.IsCompleted ?? false;
        if ((DateTime.UtcNow - _lastOnlineInstanceTime).TotalSeconds > MIN_SECONDS_BEFORE_UPDATE && !hasRunningTask)
        {
            _currentOnlineInstanceTask = _skyFrostInterfaceClient.NotifyOnlineInstance();
            _lastOnlineInstanceTime = DateTime.UtcNow;
        }

        return true;
    }

    public void FinishUpdate() { }

    public void OnlineStatusChanged(OnlineStatus status)
    {
        LoadingOnlineStatus = false;
        if (_skyFrostInterfaceClient.ForceInvisible) { return; }

        _skyFrostInterfaceClient.CurrentOnlineStatus = status;
    }

    public void SignIn() => LoadingOnlineStatus = true;

    public Task SignOut() => Task.CompletedTask;

    public bool UpdateSessions(UserStatus _0, bool _1) => true;
}
