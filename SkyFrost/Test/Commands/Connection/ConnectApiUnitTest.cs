using SkyFrost.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test.Commands.Connection;

using SkyFrost.Clients.Abstract;
using SkyFrost.Commands.Connection;

[ExcludeFromCodeCoverage]
public class ConnectApiUnitTest
{
    [Fact]
    public void ExecuteCmdlet_CredentialsDeclared_ConnectClientToApi()
    {
        MockCommandRuntime<ISkyFrostInterfaceClient> runtime = new();
        Mock<ISkyFrostInterfaceClient> mockSkyFrostClient = new();
        PSCredential mockPsCredential = new(new PSObject());

        mockSkyFrostClient
            .Setup(m => m.Login(It.IsAny<PSCredential>(), It.IsAny<string>()))
            .Returns(() => Task.CompletedTask);


        ConnectApi cmdlet = new()
        {
            CommandRuntime = runtime,
            LoginAsAnonymous = true,
            Credential = mockPsCredential,
            CreateClient = (string _1, string _2, bool _3, SkyFrostConfig _4) => mockSkyFrostClient.Object
        };

        cmdlet.StartProcessExecution();

        mockSkyFrostClient.Verify(m => m.Login(It.Is<PSCredential>(p => p == mockPsCredential), It.IsAny<string>()));

        Assert.Equal(mockSkyFrostClient.Object, SkyFrostInterfacePool.Current);
    }

    [Fact]
    public void ExecuteCmdlet_ReturnClientDeclared_ReturnsClient()
    {
        MockCommandRuntime<ISkyFrostInterfaceClient> runtime = new();
        Mock<ISkyFrostInterfaceClient> mockSkyFrostClient = new();

        ConnectApi cmdlet = new()
        {
            CommandRuntime = runtime,
            LoginAsAnonymous = true,
            ReturnClient = true,
            CreateClient = (string _1, string _2, bool _3, SkyFrostConfig _4) => mockSkyFrostClient.Object
        };

        cmdlet.StartProcessExecution();


        var actualClient = runtime.Output.First();
        Assert.NotEqual(SkyFrostInterfacePool.Current, actualClient);
        Assert.Equal(mockSkyFrostClient.Object, actualClient);
    }

    [Fact]
    public void ExecuteCmdlet_NullCredentials_ThrowsError()
    {
        MockCommandRuntime<ISkyFrostInterfaceClient> runtime = new();
        Mock<ISkyFrostInterfaceClient> mockSkyFrostClient = new();

        ConnectApi cmdlet = new()
        {
            CommandRuntime = runtime,
            Credential = null,
            CreateClient = (string _1, string _2, bool _3, SkyFrostConfig _4) => mockSkyFrostClient.Object
        };

        Assert.Throws<PSInvalidOperationException>(cmdlet.StartProcessExecution);
    }

    [Fact]
    public void ExecuteCmdlet_AnonymousDeclared_ConnectsClientToApi()
    {
        MockCommandRuntime<ISkyFrostInterfaceClient> runtime = new();
        Mock<ISkyFrostInterfaceClient> mockSkyFrostClient = new();

        mockSkyFrostClient
            .Setup(m => m.Login(It.IsAny<PSCredential>(), It.IsAny<string>()))
            .Callback(() => Assert.Fail("Credentials should be null"));


        ConnectApi cmdlet = new()
        {
            CommandRuntime = runtime,
            LoginAsAnonymous = true,
            CreateClient = (string _1, string _2, bool _3, SkyFrostConfig _4) => mockSkyFrostClient.Object
        };

        cmdlet.StartProcessExecution();

        Assert.Equal(mockSkyFrostClient.Object, SkyFrostInterfacePool.Current);
    }
}
