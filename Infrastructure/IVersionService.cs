namespace MbsCore.VersionLeapSync
{
    public interface IVersionService
    {
        IVersionChangeInfo VersionChangeInfo { get; }
        int CurrentVersionNumber { get; }
        int LastVersionNumber { get; }
        VersionComparisonResult VersionComparisonResult { get; }
        
        void Initialize();
        bool TryCompare(VersionComparer comparer);
    }
}
