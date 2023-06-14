using CloudX.Shared;
using CodeX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Clients.Abstract
{
    public interface ICloudInterfaceClient
    {
        Task<Tuple<IEnumerable<IRecord>, bool>> FindRecords(SearchParameters searchParameters);

        Task<Stream> GatherAsset(string hashId);

        Task<AssetInfo> GetGlobalAssetInfo(string hashId);

        Task<AssetInfo> GetOwnedAssetInfo(string hashId, string ownerId = "");

        Task<string> GetAssetMime(string hashId);

        Task<IEnumerable<IAssetVariantDescriptor>> GetAvailableAssetVariants(string hashId);

        Task Login(PSCredential credentials);

        Task Logout();

        Task<bool> IsPingable();
    }
}
