# UsersManager

Get-ResUser [-UserId] <string> [-ByName <SwitchParameter>]
Find-ResUser [-Query] <string>
Request-ResRecoveryCode [-Email] <string>
Get-ResCurrentUser
Get-ResFundingEvents [-User] <UserPipeBind>
Get-ResExitMessages [-User] <UserPipeBind>

# StorageManager

Get-ResStorage [-Owner] <OwnerPipeBind> [-Member <UserPipeBind>]

# SecurityManager

Request-ResOneTimeVerificationKey [-KeyType] <VerificationKeyUse> [-BaseKeyId <string>]

# ProfileManager

Get-ResUserProfile [-User] <UserPipeBind>
Set-ResUserProfile [-User] <UserPipeBind> [-IconUrl <string>] [-Tagline <string>] [-DisplayBadges <EntityId[]>] [-Description <string>] [-Values <Hashtable>]
Get-ResFavorite [-User] <UserPipeBind> [-Favorite] <FavoriteEntity>
Set-ResFavorite [-User] <UserPipeBind> [-Favorite] <FavoriteEntity> [-Uri] <Uri>

# StatisticsManager

Get-ResCloudHealth
Get-ResStatistics [-Server <SwitchParameter>] [-Online <SwitchParameter>]
Get-ResUserCredit [-CreditType] <CreditType>
Test-ResPing
Assert-ResOnlineInstance [-MachineId] <string>

# SessionsManager

Get-ResSession [-CompatibilityHash <string>] [-Name <string>] [-UniverseId <string>] [-HostName <string>] [-HostId <string>] [-MinActiveUsers <int>] [-ExcludeEmptyHeadless <SwitchParameter>]
Get-ResSession [-Id] <string>
Set-ResSessionMetadata [-Session] <SessionPipeBind> [-AccessLevel] <SessionAccessLevel> [-Hidden <SwitchParameter>]
Get-ResSessionUrls [-Session] <SessionPipeBind>

# CloudVariableManager

# UserStatusManager

# ContactManager

# GroupsManager

# MessageManager

# MigrationManager

# RecordsManager

# BadgeManager

# AppsManager

Get-ResSamlProviders [-UniverseId <string>]

# AssetInterface