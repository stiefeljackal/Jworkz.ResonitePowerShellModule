using SkyFrost.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test.Commands.Abstract;

using Core.Models.Abstract;
using SkyFrost.Clients.Abstract;
using SkyFrost.Commands.Abstract;
using SkyFrost.PipeBinds;

[ExcludeFromCodeCoverage]
public class ResoniteAssetInfoCmdletUnitTest
{
    [Theory]
    [MemberData(nameof(ValidAssetInfoData))]
    public void ExecuteCmd_ValidAssetInfo_CanProcess(AssetInfoPipeBind assetInfoPipeBindMock)
    {
        var cmdlet = SetupCmdlet(assetInfoPipeBindMock);

        try
        {
            cmdlet.StartProcessExecution();
            Assert.Equal(assetInfoPipeBindMock, cmdlet.AssetInfo);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    [Theory]
    [MemberData(nameof(InvalidAssetInfoData))]
    public void ExecuteCmd_InvalidAssetInfo_ThrowsError(AssetInfoPipeBind assetInfoPipeBindMock)
    {
        var cmdlet = SetupCmdlet(assetInfoPipeBindMock);

        Assert.Throws<PSInvalidOperationException>(cmdlet.StartProcessExecution);
    }

    [Fact]
    public void CreateCmd_FileSystemDI_UsesCustomFileSystem()
    {
        var fileSystemMock = new Mock<IFileSystem>();
        var cmdlet = new Mock<ResoniteAssetInfoCmdlet>(fileSystemMock.Object) { CallBase = true }.Object;
        Assert.Equal(fileSystemMock.Object, cmdlet.FileSystem);
    }

    /// <summary>
    /// Configures and returns a mock instance of the <see cref="ResoniteAssetInfoCmdlet"/> class for testing purposes.
    /// </summary>
    /// <param name="assetInfoPipeBindMock">A mock implementation of <see cref="AssetInfoPipeBind"/> to be assigned to the <see
    /// cref="ResoniteAssetInfoCmdlet.AssetInfo"/> property.</param>
    /// <param name="callBase">A value indicating whether the base class implementation of the mocked cmdlet should be called.  Defaults to
    /// <see langword="true"/>.</param>
    /// <returns>A fully configured mock instance of <see cref="ResoniteAssetInfoCmdlet"/> with its properties and dependencies
    /// set up.</returns>
    private static ResoniteAssetInfoCmdlet SetupCmdlet(AssetInfoPipeBind assetInfoPipeBindMock, bool callBase = true)
    {
        Mock<ISkyFrostInterfaceClient> skyFrostClientMock = new();
        Mock<ResoniteAssetInfoCmdlet> cmdletMock = new();
        cmdletMock.CallBase = callBase;

        var cmdlet = cmdletMock.Object;

        cmdlet.CommandRuntime = new MockCommandRuntime<object>();
        cmdlet.Client = skyFrostClientMock.Object;
        cmdlet.AssetInfo = assetInfoPipeBindMock;

        return cmdlet;
    }

    public static IEnumerable<object?[]> ValidAssetInfoData =>
        [
            [new AssetInfoPipeBind(GlobalConstants.MOCK_HASH)],
            [GlobalConstants.MOCK_HASH],
            [new AssetInfoPipeBind(new AssetInfo { AssetHash = GlobalConstants.MOCK_HASH })],
            [new AssetInfo { AssetHash = GlobalConstants.MOCK_HASH }]
        ];

    public static IEnumerable<object?[]> InvalidAssetInfoData =>
        [
            [new AssetInfoPipeBind("")],
            [""],
            [null],
            [new AssetInfoPipeBind(hashId: null)],
            [new AssetInfoPipeBind(new AssetInfo { AssetHash = "" })],
            [new AssetInfo { AssetHash = "" }],
            [new AssetInfoPipeBind(new AssetInfo { AssetHash = null })],
            [new AssetInfo { AssetHash = null }]
        ];
}
