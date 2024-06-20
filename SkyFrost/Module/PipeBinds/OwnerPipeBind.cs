using SkyFrost.Base;
using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.PipeBinds;

using Clients.Abstract;
using Models;

public sealed class OwnerPipeBind
{
    private readonly string? _ownerId;
    private OwnerType? _ownerType;
    private Owner? _owner;

    /// <summary>
    /// Id of the owner
    /// </summary>
    public string OwnerId => _owner?.Id ?? _ownerId ?? string.Empty;

    /// <summary>
    /// Declared type of the owner
    /// </summary>
    public OwnerType OwnerType => _ownerType ??= IdUtil.GetOwnerType(OwnerId);

    public OwnerPipeBind(string ownerId)
    {
        _ownerId = ownerId ?? throw new ArgumentNullException("ownerId");
    }

    public OwnerPipeBind(User user)
    {
        _owner = new Owner(user);
    }

    public OwnerPipeBind(Group group)
    {
        _owner = new Owner(group);
    }

    public bool IsValidOwnerId() => OwnerType != OwnerType.INVALID && OwnerType != OwnerType.Machine;


    internal async Task<Owner?> GetOwner(ISkyFrostInterfaceClient? client)
    {
        if (_owner != null) { return _owner; }

        if (client == null) { throw new ArgumentNullException(nameof(client)); }

        if (_ownerId == client.CurrentUser.Id) { return _owner = new Owner(client.CurrentUser); }

        switch(OwnerType)
        {
            case OwnerType.User: return _owner = new Owner(await client.GetUser(_ownerId!));
            case OwnerType.Group: return _owner = new Owner(await client.GetGroup(_ownerId!));
            default: return null;
        }
    }

    public static implicit operator OwnerPipeBind(string ownerId) => new OwnerPipeBind(ownerId);

    public static implicit operator OwnerPipeBind(User owner) => new OwnerPipeBind(owner);

    public static implicit operator OwnerPipeBind(Group owner) => new OwnerPipeBind(owner);
}
