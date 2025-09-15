using System.Management.Automation;
using Elements.Assets;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Clients.Abstract;

public interface ISkyFrostInterfaceClient
{
    User CurrentUser { get; }

    IPlatformProfile PlatformProfile { get; }

    IUserStatusSource StatusSource { get; set; }

    OnlineStatus CurrentOnlineStatus { get; set; }

    bool ForceInvisible { get; set; }

    Task<Tuple<IEnumerable<IRecord>, bool>> FindRecords(SearchParameters searchParameters);

    Task<Stream> GatherAsset(string hashId);

    Task<AssetInfo> GetGlobalAssetInfo(string hashId);

    Task<AssetInfo> GetOwnedAssetInfo(string hashId, string? ownerId = "");

    Task<Record> GetRecord(RecordId recordId);

    Task<Record> GetRecord(string ownerId, string rid);

    Task<Record> GetRecord(Uri recordUri);

    Task<IEnumerable<Record>> GetRecordsByOwner(string ownerId, string? tag = null, string? path = null);

    IAsyncEnumerable<Record> GetRecordsInHierarchy(string ownerId, string path);

    Task<Record> GetRecordAtPath(string ownerId, string path, string? accessKey = null);

    Task<IEnumerable<Record>> GetRecords(IEnumerable<RecordId> recordIds);

    Task<string> GetAssetMime(string hashId);

    Task<IEnumerable<IAssetVariantDescriptor>> GetAvailableAssetVariants(string hashId);

    Task<User> GetUser(string userId);

    Task<Group> GetGroup(string groupId);

    Task Login(PSCredential credentials, string totp = "");

    Task Logout();

    Task NotifyOnlineInstance();

    Task<bool> IsPingable();
}
