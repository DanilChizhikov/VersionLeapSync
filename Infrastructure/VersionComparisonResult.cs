namespace MbsCore.VersionLeapSync
{
    public enum VersionComparisonResult : byte
    {
        Same = 0,
        Upgrade = 1,
        Downgrade = 2,
    }
}