using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test;

internal static class GlobalConstants
{
    public const string MOCK_USER_ID = "U-Mock";
    public const string MOCK_USER_USERNAME = "User Mock";
    public const string MOCK_GROUP_ID = "G-Mock";
    public const string MOCK_GROUP_NAME = "Group Mock";
    public const string MOCK_MACHINE_ID = "M-Mock";
    public const string MOCK_RECORD_ID = "R-00000000-0000-4000-0000-000000000000";

    public static readonly User MOCK_USER = new()
    {
        Id = MOCK_USER_ID,
        Username = MOCK_USER_USERNAME
    };

    public static readonly Group MOCK_GROUP = new()
    {
        GroupId = MOCK_GROUP_ID,
        Name = MOCK_GROUP_NAME
    };
}
