using JworkzNeosPowerShellModule.Clients.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CloudX.Shared;
using JworkzNeosPowerShellModule.Utilities;
using System.Text.RegularExpressions;
using CodeX;

namespace JworkzNeosPowerShellModule.Clients
{
    public class CloudXInterfaceClient : ICloudInterfaceClient
    {
        private CloudXInterface _cloudXInterface;

        public string Uid { get => _cloudXInterface?.UID ?? string.Empty; }

        public string SecretMachineId { get; private set; } = "AyDC6i3MActP";

        /// <summary>
        /// Original CloudXInterface object that the NeosVR client uses
        /// </summary>
        public CloudXInterface Raw { get => _cloudXInterface; }

        public CloudXInterfaceClient()
        {
            _cloudXInterface = new CloudXInterface(null, "Neos", "2022.1.28.1335", OperatingSystem.IsLinux());
        }

        public async Task<Tuple<IEnumerable<IRecord>,bool>> FindRecords(SearchParameters searchParameters)
        {
            var searchResults = await Raw.FindRecords<Record>(searchParameters);

            if (searchResults.IsError)
            {
                throw new Exception(searchResults.Content);
            }

            var recordsEnumerable = searchResults.Entity.Records.AsEnumerable();

            return new Tuple<IEnumerable<IRecord>, bool>(recordsEnumerable, searchResults.Entity.HasMoreResults);
        }

        public async Task<Stream> GatherAsset(string hashId)
        {
            var stream = await Raw.GatherAsset(hashId);
            return stream;
        }

        public async Task<AssetInfo> GetGlobalAssetInfo(string hashId)
        {
            var result = await Raw.GetGlobalAssetInfo(hashId);

            CheckCloudResult(result, "Unable to fetch asset info");

            return result.Entity;
        }

        public async Task<AssetInfo> GetOwnedAssetInfo(string hashId, string? ownerId = "")
        {
            var result = string.IsNullOrEmpty(ownerId)
                ? await Raw.GetUserAssetInfo(hashId)
                : await Raw.GetAssetInfo(ownerId, hashId);

            CheckCloudResult(result, "Unable to fetch asset info");

            return result.Entity;
        }

        public async Task<string> GetAssetMime(string hashId)
        {
            var result = await _cloudXInterface.GetAssetMime(hashId);

            return result.Content;
        }

        public async Task<IEnumerable<IAssetVariantDescriptor>> GetAvailableAssetVariants(string hashId)
        {
            /*var result = await _cloudXInterface.GetAvailableVariants(hashId);
            var strVariants = result.Entity;
            var queryParamPair = new Dictionary<string, string>();

            foreach (var strVariant in strVariants)
            {
                var matches = StringExtensions.QUERY_PARAM_REGEX.Matches(strVariant);
                foreach(var match in matches)
                {
                    var t = new Texture2DVariantDescriptor();
                    
                    var c = new CubemapVariantDescriptor();

                    var s = new ShaderVariantDescriptor();

                    var m = new MeshVariantDescriptor();
                    m.DataType
                }
            }

            StringExtensions.QUERY_PARAM_REGEX.Matches();

            return result.Entity;*/
            throw new NotImplementedException();
        }

        public async Task Login(PSCredential credential)
        {
            var secretMachineId = Guid.NewGuid().ToString();

            try
            {
                await _cloudXInterface.Login(
                    credential.UserName,
                    credential.Password.ToPlainText(),
                    null,
                    secretMachineId,
                    false,
                    null,
                    null
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
            await _cloudXInterface.Logout(true).ConfigureAwait(false);

            _cloudXInterface.HttpClient?.Dispose();
            _cloudXInterface.SafeHttpClient?.Dispose();
            SecretMachineId = string.Empty;
        }
    
        public async Task<bool> IsPingable()
        {
            var result = await Raw.Ping();
            return result.IsOK;
        }

        private void CheckCloudResult(CloudResult cloudResult, string msg = "")
        {
            if (cloudResult.IsError)
            {
                var content = cloudResult.Content;
                var state = cloudResult.State;
                var contentMsg = content != null ? $" | {content}" : string.Empty;
                throw new Exception($"{(string.IsNullOrEmpty(msg) ? "NeosVR returned a cloud error for this request" : msg)}: {state}{contentMsg}");
            }
        }
    }
}
