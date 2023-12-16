using System.Management.Automation;
using Elements.Assets;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.Core.Clients.Abstract;

public interface ISkyFrostInterfaceClient
{
    User CurrentUser { get; }

    IUserStatusSource StatusSource { get; set; }

    OnlineStatus CurrentOnlineStatus { get; set; }

    bool ForceInvisible { get; set; }

    Task<Tuple<IEnumerable<IRecord>, bool>> FindRecords(SearchParameters searchParameters);

    Task<Stream> GatherAsset(string hashId);

    Task<AssetInfo> GetGlobalAssetInfo(string hashId);

    Task<AssetInfo> GetOwnedAssetInfo(string hashId, string ownerId = "");

    Task<Record> GetRecord(string ownerId, string recordId);

    Task<Record> GetRecord(Uri recordUri);

    Task<IEnumerable<Record>> GetRecordsAtPath(string ownerId, string path);

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
