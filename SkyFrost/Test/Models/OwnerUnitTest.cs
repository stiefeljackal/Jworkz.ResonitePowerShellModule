using SkyFrost.Base;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test.Models;

using SkyFrost.Models;

public class OwnerUnitTest
{
    [Fact]
    public void CreateOwnerModel_NullUser_ThrowsArgumentNullException()
    {
        User? mockUser = null;

#pragma warning disable CS8604 // Possible null reference argument.
        Assert.Throws<ArgumentNullException>(() => new Owner(mockUser));
#pragma warning restore CS8604 // Possible null reference argument.
    }

    [Fact]
    public void CreateOwnerModel_NullGroup_ThrowsArgumentNullException()
    {
        Group? mockGroup = null;

#pragma warning disable CS8604 // Possible null reference argument.
        Assert.Throws<ArgumentNullException>(() => new Owner(mockGroup));
#pragma warning restore CS8604 // Possible null reference argument.
    }

    [Fact]
    public void CreateOwnerModel_UserOwnerObjectWithSamePropertyValues_DoesEqualToOtherOwnerOfSameValues()
    {
        Owner expectedOwner = new (GlobalConstants.MOCK_USER);
        Owner actualOwner = new(GlobalConstants.MOCK_USER);
        
        Assert.True(expectedOwner.Equals(actualOwner));
    }

    [Fact]
    public void CreateOwnerModel_GroupOwnerObjectWithSamePropertyValues_DoesEqualToOtherOwnerOfSameValues()
    {
        Owner expectedOwner = new(GlobalConstants.MOCK_GROUP);
        Owner actualOwner = new(GlobalConstants.MOCK_GROUP);

        Assert.True(expectedOwner.Equals(actualOwner));
    }

    [Fact]
    public void CreateOwnerModel_UserOwnerObjectWithDifferentPropertyValues_DoesNotEqualToOtherOwner()
    {
        Owner leftOwner = new(GlobalConstants.MOCK_USER);
        Owner rightOwner = new(
            new User
            {
                Id = "U-OtherUser",
                Username = "Other"
            }
        );

        Assert.False(leftOwner.Equals(rightOwner));
        Assert.False(leftOwner == rightOwner);
    }

    [Fact]
    public void CreateOwnerModel_GroupOwnerObjectWithDifferentPropertyValues_DoesNotEqualToOtherOwner()
    {
        Owner leftOwner = new(GlobalConstants.MOCK_GROUP);
        Owner rightOwner = new(
            new Group
            {
                GroupId = GlobalConstants.MOCK_GROUP_ID,
                Name = "Other Group"
            }
        );

        Assert.False(leftOwner.Equals(rightOwner));
        Assert.False(leftOwner == rightOwner);
    }

    [Fact]
    public void CreateOwnerModel_GroupOwnerObject_DoesNotEqualToUserOwner()
    {
        Owner leftOwner = new(GlobalConstants.MOCK_GROUP);
        Owner rightOwner = new(
            new User
            {
                Id = GlobalConstants.MOCK_GROUP_ID,
                Username = GlobalConstants.MOCK_GROUP_NAME
            }
        );

        Assert.False(leftOwner.Equals(rightOwner));
        Assert.True(leftOwner != rightOwner);
    }

    [Fact]
    public void CreateOwnerModel_StringIdHashCode_EqualsToOwnerObjectHashCode()
    {
        Owner owner = new(GlobalConstants.MOCK_GROUP);
        var hashCode = GlobalConstants.MOCK_GROUP_ID.GetHashCode();

        Assert.Equal(hashCode, owner.GetHashCode());
    }

    [Theory]
    [InlineData(null)]
    [InlineData(3)]
    [InlineData("string")]
    [InlineData('a')]
    [InlineData(98.6)]
    public void CreateOwnerModel_NonOwnerObject_DoesNotEqualToOwner(object? owner)
    {
        Owner mockOwner = new Owner(new Mock<User>().Object);
        Assert.False(mockOwner.Equals(owner));
    }
}
