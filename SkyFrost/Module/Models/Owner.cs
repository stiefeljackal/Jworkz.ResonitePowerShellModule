using SkyFrost.Base;
using System.Diagnostics.CodeAnalysis;

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

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj == null || !(obj is Owner)) {  return false; }

        var otherOwner = (Owner)obj;
        
        return Id == otherOwner.Id && Name == otherOwner.Name && OwnerType == otherOwner.OwnerType;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Owner left, Owner right) => left.Equals(right);

    public static bool operator !=(Owner left, Owner right) => !left.Equals(right);
}
