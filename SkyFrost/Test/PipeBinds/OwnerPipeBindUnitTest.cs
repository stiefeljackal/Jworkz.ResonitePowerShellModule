using Moq;
using SkyFrost.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test.PipeBinds;

using SkyFrost.Clients.Abstract;
using SkyFrost.Models;
using SkyFrost.PipeBinds;

public class OwnerPipeBindUnitTest
{
    [Theory]
    [InlineData(GlobalConstants.USER_OWNER_ID, OwnerType.User)]
    [InlineData(GlobalConstants.GROUP_OWNER_ID, OwnerType.Group)]
    public void TakePipeBind_ValidOwnerId_IsValidOwner(string expectedOwnerId, OwnerType expectedOwnerType)
    {
        OwnerPipeBind ownerMock = new(expectedOwnerId);

        Assert.True(ownerMock.IsValidOwnerId());
        Assert.Equal(expectedOwnerType, ownerMock.OwnerType);
        Assert.Equal(expectedOwnerId, ownerMock.OwnerId);
    }

    [Fact]
    public void TakePipeBind_ValidOwnerObject_IsValidOwner()
    {
        User userMock = new() { Id = GlobalConstants.USER_OWNER_ID };
        OwnerPipeBind ownerMock = new(userMock);

        Assert.True(ownerMock.IsValidOwnerId());
        Assert.Equal(OwnerType.User, ownerMock.OwnerType);
        Assert.Equal(GlobalConstants.USER_OWNER_ID, ownerMock.OwnerId);
    }

    [Fact]
    public void TakePipeBind_ValidGroupObject_IsValidOwner()
    {
        Group groupMock = new() { GroupId = GlobalConstants.GROUP_OWNER_ID };
        OwnerPipeBind ownerMock = new(groupMock);

        Assert.True(ownerMock.IsValidOwnerId());
        Assert.Equal(OwnerType.Group, ownerMock.OwnerType);
        Assert.Equal(GlobalConstants.GROUP_OWNER_ID, ownerMock.OwnerId);
    }

    [Theory]
    [InlineData("", OwnerType.INVALID)]
    [InlineData(GlobalConstants.MACHINE_ID, OwnerType.Machine)]
    public void TakePipeBind_InvalidOwner_IsInvalidOwner(string expectedOwnerId, OwnerType expectedOwnerType)
    {
        OwnerPipeBind ownerMock = new(expectedOwnerId);

        Assert.False(ownerMock.IsValidOwnerId());
        Assert.Equal(expectedOwnerType, ownerMock.OwnerType);
        Assert.Equal(expectedOwnerId, ownerMock.OwnerId);
    }

    [Fact]
    public void TakePipeBind_NullOwnerId_ThrowsException()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(() => new OwnerPipeBind(ownerId: null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public async Task CallGetOwner_UserOwner_ReturnsOwnerObject()
    {
        User user = new() { Id = GlobalConstants.USER_OWNER_ID };
        OwnerPipeBind ownerMock = new(user);

        var owner = await ownerMock.GetOwner(null);
        Assert.Equal(new Owner(user), owner);
    }

    [Fact]
    public async Task CallGetOwner_GroupOwner_ReturnsOwnerObject()
    {
        Group group = new() { GroupId = GlobalConstants.GROUP_OWNER_ID };
        OwnerPipeBind ownerMock = new(group);

        var owner = await ownerMock.GetOwner(null);
        Assert.Equal(new Owner(group), owner);
    }

    [Fact]
    public async Task CallGetOwner_NoOwnerObjectAndNullClient_ThrowsException()
    {
        OwnerPipeBind ownerMock = new (GlobalConstants.USER_OWNER_ID);

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await ownerMock.GetOwner(null));
    }

    [Fact]
    public async Task CallGetOwner_OwnerIsCurrentUser_ReturnsOwnerObject()
    {
        User currentUser = new() { Id = GlobalConstants.USER_OWNER_ID };
        Mock<ISkyFrostInterfaceClient> skyFrostClientMock = new();

        skyFrostClientMock.Setup(m => m.CurrentUser).Returns(() => currentUser);

        OwnerPipeBind ownerMock = new(currentUser.Id);

        var owner = await ownerMock.GetOwner(skyFrostClientMock.Object);

        Assert.NotNull(owner);
        Assert.Equal(new Owner(currentUser), owner);
        skyFrostClientMock.Verify(m => m.CurrentUser, Times.AtLeastOnce);
    }

    [Fact]
    public async Task CallGetOwner_UserOwnerId_ReturnsOwnerObject()
    {
        User user = new() { Id = GlobalConstants.USER_OWNER_ID };
        Mock<ISkyFrostInterfaceClient> skyFrostClientMock = new();

        skyFrostClientMock.Setup(m => m.CurrentUser).Returns(() => new User());
        skyFrostClientMock
            .Setup(m => m.GetUser(It.Is<string>(i => i == GlobalConstants.USER_OWNER_ID)))
            .Returns(() => Task.FromResult(user));

        OwnerPipeBind ownerPipeBind = new(GlobalConstants.USER_OWNER_ID);
        var owner = await ownerPipeBind.GetOwner(skyFrostClientMock.Object);

        Assert.NotNull(owner);
        Assert.Equal(new Owner(user), owner);
        skyFrostClientMock.Verify(m => m.GetUser(It.Is<string>(i => i == GlobalConstants.USER_OWNER_ID)), Times.Once);
    }

    [Fact]
    public async Task CallGetOwner_GroupOwnerId_ReturnsOwnerObject()
    {
        Group group = new() { GroupId = GlobalConstants.GROUP_OWNER_ID };
        Mock<ISkyFrostInterfaceClient> skyFrostClientMock = new();

        skyFrostClientMock.Setup(m => m.CurrentUser).Returns(() => new User());
        skyFrostClientMock
            .Setup(m => m.GetGroup(It.Is<string>(i => i == GlobalConstants.GROUP_OWNER_ID)))
            .Returns(() => Task.FromResult(group));

        OwnerPipeBind ownerPipeBind = new(GlobalConstants.GROUP_OWNER_ID);
        var owner = await ownerPipeBind.GetOwner(skyFrostClientMock.Object);

        Assert.NotNull(owner);
        Assert.Equal(new Owner(group), owner);
        skyFrostClientMock.Verify(m => m.GetGroup(It.Is<string>(i => i == GlobalConstants.GROUP_OWNER_ID)), Times.Once);
    }

    [Theory]
    [InlineData("Mock")]
    [InlineData(GlobalConstants.MACHINE_ID)]
    public async Task CallGetOwner_InvalidOwnerId_ReturnsNull(string? id)
    {
        Mock<ISkyFrostInterfaceClient> skyFrostClientMock = new();

        skyFrostClientMock.Setup(m => m.CurrentUser).Returns(() => new User());

        OwnerPipeBind ownerPipeBind = new(id ?? string.Empty);
        var owner = await ownerPipeBind.GetOwner(skyFrostClientMock.Object);

        Assert.Null(owner);
    }

    public static IEnumerable<object?[]> InvalidOwnerData =>
        new object?[][]
        {
            ["Mock", OwnerType.INVALID],
            [GlobalConstants.MACHINE_ID, OwnerType.Machine]
        };
}
