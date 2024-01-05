using System.Management.Automation;
using SkyFrost.Base;
using Elements.Assets;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Clients;

using Abstract;
using Core.Utilities;

public class SkyFrostInterfaceClient : ISkyFrostInterfaceClient
{
    private SkyFrostInterface _skyfrostInterface;

    public readonly string Uid = UID.Compute();

    /// <summary>
    /// Original SkyFrostInterface object that the Resonite client uses
    /// </summary>
    public SkyFrostInterface Raw { get => _skyfrostInterface; }

    public User CurrentUser => _skyfrostInterface.CurrentUser;

    public OnlineStatus CurrentOnlineStatus
    {
        get => _skyfrostInterface.Status.OnlineStatus;
        set
        {
            var status = _skyfrostInterface.Status;
            if (status.OnlineStatus != value)
            {
                _skyfrostInterface.Status.OnlineStatus = value;
                _skyfrostInterface.Update();
            }
        }
    }

    public IUserStatusSource StatusSource
    {
        get => _skyfrostInterface.Status.StatusSource;
        set => _skyfrostInterface.Status.StatusSource = value;
    }

    public bool ForceInvisible
    {
        get => _skyfrostInterface.Status.ForceInvisible;
        set => _skyfrostInterface.Status.ForceInvisible = value;
    }

    public SkyFrostInterfaceClient(SkyFrostConfig config, string productName, string version, bool disableSignalR = false)
    {
        if (disableSignalR)
        {
            config.WithoutSignalR();
        }
        config.WithGzip(Platform.IsWindows);
        config.WithUserAgent(productName.Replace(' ', '_'), version);

        _skyfrostInterface = new SkyFrostInterface(Uid, config);
        _skyfrostInterface.Api.DefaultRetries = 0;
    }

    public async Task<Tuple<IEnumerable<IRecord>,bool>> FindRecords(SearchParameters searchParameters)
    {
        var searchResults = await Raw.Records.FindRecords<Record>(searchParameters);

        if (searchResults.IsError)
        {
            throw new Exception(searchResults.Content);
        }

        var recordsEnumerable = searchResults.Entity.Records.AsEnumerable();

        return new Tuple<IEnumerable<IRecord>, bool>(recordsEnumerable, searchResults.Entity.HasMoreResults);
    }

    public async Task<Stream> GatherAsset(string hashId)
    {
        var downloadUrl = Raw.Assets.DBToHttp(Raw.Assets.GenerateURL(hashId), DB_Endpoint.Default);
        var stream = await Raw.SafeHttpClient.GetStreamAsync(downloadUrl);
        return stream;
    }

    public async Task<AssetInfo> GetGlobalAssetInfo(string hashId)
    {
        var result = await Raw.Assets.GetGlobalAssetInfo(hashId);

        CheckCloudResult(result, "Unable to fetch asset info");

        return result.Entity;
    }

    public async Task<AssetInfo> GetOwnedAssetInfo(string hashId, string? ownerId = "")
    {
        var result = string.IsNullOrEmpty(ownerId)
            ? await Raw.Assets.GetUserAssetInfo(hashId)
            : await Raw.Assets.GetAssetInfo(ownerId, hashId);

        CheckCloudResult(result, "Unable to fetch asset info");

        return result.Entity;
    }

    public async Task<Record> GetRecord(string ownerId, string recordId)
    {
        var result = await Raw.Records.GetRecord<Record>(ownerId, recordId);

        CheckCloudResult(result, "Unable to fetch record");

        return result.Entity;
    }

    public async Task<Record> GetRecord(Uri recordUri)
    {
        var result = await Raw.Records.GetRecord<Record>(recordUri);

        CheckCloudResult(result, $"Unable to fetch '{recordUri}' from Cloud");

        return result.Entity;
    }

    public async Task<IEnumerable<Record>> GetRecords(IEnumerable<RecordId> recordIds)
    {
        var result = await Raw.Records.GetRecords<Record>(recordIds.ToList());

        CheckCloudResult(result, "Unable to fetch records from Cloud");

        return result.Entity;
    }

    public async Task<IEnumerable<Record>> GetRecordsAtPath(string ownerId, string path)
    {
        var result = await Raw.Records.GetRecords<Record>(ownerId, path: path);
        CheckCloudResult(result, "Unable to fetch the full record data");

        return result.Entity;
    }

    public async Task<string> GetAssetMime(string hashId)
    {
        var result = await _skyfrostInterface.Assets.GetAssetMime(hashId);

        return result.Content;
    }

    public async Task<IEnumerable<IAssetVariantDescriptor>> GetAvailableAssetVariants(string hashId)
    {
        var result = await _skyfrostInterface.Assets.GetAvailableVariants(hashId);
        var strVariants = result.Entity;
        var queryParamPair = new Dictionary<string, string>();

        List<IAssetVariantDescriptor> variants = new();

        foreach (var strVariant in strVariants)
        {
            var matches = StringExtensions.QueryParamRegex.Matches(strVariant);

            if (matches.Any(m => m.Groups["key"].Value == "filtering"))
            {
                variants.Add(Texture2DVariantDescriptor.FromIdentifier(strVariant));
            }
            else if (matches.Any(m => m.Groups["key"].Value == "size"))
            {
                variants.Add(CubemapVariantDescriptor.FromIdentifier(strVariant));
            }
            else if (matches.Any(m => m.Groups["key"].Value == "platform"))
            {
                variants.Add(ShaderVariantDescriptor.FromIdentifier(strVariant));
            }
            else if (matches.Any(m => m.Groups["key"].Value == "datatype"))
            {
                variants.Add(MeshVariantDescriptor.FromIdentifier(strVariant));
            }
        }

        return variants;
    }

    public async Task<User> GetUser(string userId)
    {
        var user = (await _skyfrostInterface.Users.GetUser(userId)).Entity;
        return user;
    }

    public async Task<Group> GetGroup(string groupId)
    {
        var group = (await _skyfrostInterface.Groups.GetGroup(groupId)).Entity;
        return group;
    }

    public async Task Login(PSCredential credential, string totp = "")
    {
        var secretMachineId = Guid.NewGuid().ToString();
        PasswordLogin loginAuth = new()
        {
            Password = credential.Password.ToPlainText(),
        };

        try
        {
            await _skyfrostInterface.Session.Login(
                credential.UserName,
                loginAuth,
                secretMachineId,
                false,
                totp
            ).ConfigureAwait(false);
        }
        catch
        {
            await Logout().ConfigureAwait(false);

            throw;
        }
    }

    public async Task Logout()
    {
        await _skyfrostInterface.Session.Logout(true).ConfigureAwait(false);

        await _skyfrostInterface.HubStatusController.SignOut();
        _skyfrostInterface.Api.Client?.Dispose();
        _skyfrostInterface.SafeHttpClient?.Dispose();
    }

    public async Task NotifyOnlineInstance()
    {
        await _skyfrostInterface.Stats.NotifyOnlineInstance(Uid);
    }

    public async Task<bool> IsPingable()
    {
        try
        {
            await Raw.HubClient.Ping(0).ConfigureAwait(false);
        }
        catch
        {
            return false;
        }
        return true;
    }

    private void CheckCloudResult(CloudResult cloudResult, string msg = "")
    {
        if (cloudResult.IsError)
        {
            var content = cloudResult.Content;
            var state = cloudResult.State;
            var contentMsg = content != null ? $" | {content}" : string.Empty;
            throw new Exception($"{(string.IsNullOrEmpty(msg) ? "Resonite returned a cloud error for this request" : msg)}: {state}{contentMsg}");
        }
    }
}
