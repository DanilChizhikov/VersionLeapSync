namespace MbsCore.VersionLeapSync
{
    public interface IVersionChangeInfo
    {
        int CurrentVersionNumber { get; }
        int LastVersionNumber { get; }
        VersionComparisonResult ComparisonResult { get; }
    }
}