using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.Core.PipeBinds;

using Clients.Abstract;
using Models;

public class OwnerPipeBind
{
    private readonly string? _ownerId;
    private Owner? _owner;

    public string OwnerId
    {
        get => _owner?.Id ?? _ownerId ?? string.Empty;
    }

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

    public bool IsValidOwnerId() => IdUtil.GetOwnerType(OwnerId) != OwnerType.INVALID;


    internal async Task<Owner?> GetOwner(ISkyFrostInterfaceClient? client)
    {
        if (client == null) { throw new ArgumentNullException(nameof(client)); }

        if (_owner != null) { return _owner; }

        if (_ownerId == client.CurrentUser.Id) { return _owner ??= new Owner(client.CurrentUser); }

        var ownerType = IdUtil.GetOwnerType(_ownerId);
        switch(ownerType)
        {
            case OwnerType.User: return _owner ??= new Owner(await client.GetUser(_ownerId!));
            case OwnerType.Group: return _owner ??= new Owner(await client.GetGroup(_ownerId!));
        }

        return null;
    }
}
