using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Models;

public struct Owner
{
    public readonly string Id;

    public readonly string Name;

    public OwnerType OwnerType;

    public Owner(User user)
    {
        if (user == null) { throw new ArgumentNullException("user"); }

        Id = user.Id;
        Name = user.Username;
        OwnerType = OwnerType.User;
    }

    public Owner(Group group)
    {
        if (group == null) { throw new ArgumentNullException("group"); }

        Id = group.GroupId;
        Name = group.Name;
        OwnerType = OwnerType.Group;
    }
}
