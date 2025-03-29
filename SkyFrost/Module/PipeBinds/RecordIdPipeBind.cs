using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.PipeBinds;

using Core.Utilities;
using SkyFrost.Clients.Abstract;

/// <summary>
/// Pipe bind object for record id objects or components that build a record id
/// </summary>
public class RecordIdPipeBind
{
    private Uri? _recUri;

    private RecordId? _recordId;

    private string? _ownerId;

    private string? _rid;

    /// <summary>
    /// Create the pipe bind from a rec uri string or the owner id and rid string pair
    /// </summary>
    /// <param name="recUriOrId"></param>
    public RecordIdPipeBind(string recUriOrId)
    {
        if (Uri.IsWellFormedUriString(recUriOrId, UriKind.Absolute))
        {
            _recUri = new Uri(recUriOrId);
            return;
        }
        var regexMatch = CommonRegex.RecordIdRegex.Match(recUriOrId);
        _ownerId = regexMatch.Groups["ownerId"]?.Value ?? string.Empty;
        _rid = regexMatch.Groups["rid"]?.Value ?? string.Empty;
    }

    /// <summary>
    /// Create the pipe bind from a rec uri
    /// </summary>
    /// <param name="recUri"></param>
    public RecordIdPipeBind(Uri recUri)
    {
        _recUri = recUri;
    }

    /// <summary>
    /// Create the pipe bind from an already formed record id
    /// </summary>
    /// <param name="recordId"></param>
    public RecordIdPipeBind(RecordId? recordId)
    {
        _recordId = recordId;
    }

    /// <summary>
    /// Retrieves the RecordID based on information from the pipe
    /// </summary>
    /// <param name="client">The SkyFrost client required for the chosen platform profile</param>
    /// <returns>RecordId from the pipe</returns>
    /// <exception cref="ArgumentNullException">Thrown when the ISkyFrostInterfaceClient is null</exception>
    /// <exception cref="Exception">Thrown when the record uri, owner id, or rid do not follow the proper syntax</exception>
    internal RecordId GetRecordId(ISkyFrostInterfaceClient? client)
    {
        if (client == null) { throw new ArgumentNullException(nameof(client)); }
        
        if (_recordId != null && _recordId.IsValid) { return _recordId; }

        if (_recUri != null)
        {
            var hasExtracted = client.PlatformProfile.ExtractRecordID(_recUri, out _ownerId, out _rid);

            if (!hasExtracted)
            {
                throw new Exception($"Uri '{_recUri}' does not follow the proper syntax.");
            }
        }

        if (string.IsNullOrEmpty(_ownerId))
        {
            _ownerId = client.CurrentUser?.Id;
        }

        if (!RecordHelper.IsValidRecordID(_rid))
        {
            throw new Exception($"RID '{_rid}' does not follow the proper syntax.");
        }
        if (IdUtil.GetOwnerType(_ownerId) == OwnerType.INVALID)
        {
            throw new Exception($"OwnerId '{_ownerId}' does not follow the proper syntax.");
        }

        return _recordId ??= new RecordId(_ownerId, _rid);
    }
}
