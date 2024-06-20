using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test.Commands.Connection;

using Core.Models.Abstract;
using SkyFrost.Commands.Connection;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

[ExcludeFromCodeCoverage]
public class GetSkyFrostConfigUnitTest
{
    private const string STARTING_PATH = @"C:\My Workspace\Here\Resonite";

    [Theory]
    [InlineData(@"C:\Some\Fake\Path\config.json")]
    [InlineData("D:/Another/Fake/Path/config.json")]
    [InlineData(@"\\Even/More\Fakes\config.json")]
    [InlineData("//Even/More/Fakes/config.json")]
    [InlineData(@".\Migrations\config.json")]
    [InlineData("./Migrations/config.json")]
    [InlineData(@"..\Migrations\config.json")]
    [InlineData("../Migrations/config.json")]
    public void ExecuteCmdlet_ValidPath_ReturnsValidConfig(string path)
    {
        MockCommandRuntime<SkyFrostConfig> runtime = new();
        Mock<IFileSystem> mockFileSystem = new();
        Mock<IPSState> mockPSState = new();
        SkyFrostConfig mockConfig = new();
        
        mockFileSystem
            .Setup(m => m.OpenRead(It.IsAny<string>()))
            .Returns(() => {
                MemoryStream stream = new();
                JsonSerializer.Serialize(stream, mockConfig);
                stream.Position = 0;
                return stream;
            });

        mockPSState.Setup(m => m.GetCurrentPwd()).Returns(STARTING_PATH);

        GetSkyFrostConfig cmdlet = new()
        {
            CommandRuntime = runtime,
            FileSystem = mockFileSystem.Object,
            Path = path
        };
        cmdlet.PSState = mockPSState.Object;
        
        cmdlet.StartProcessExecution();

        mockFileSystem.Verify(m => m.OpenRead(It.Is<string>(arg => arg == Path.GetFullPath(Path.Combine(STARTING_PATH, path)))));

        Assert.Equal(JsonSerializer.Serialize(mockConfig), JsonSerializer.Serialize(runtime.Output.First()));
    }
}
