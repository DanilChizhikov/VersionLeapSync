namespace MbsCore.VersionLeapSync
{
    internal sealed class VersionChangeInfo : IVersionChangeInfo
    {
        public int CurrentVersionNumber { get; }
        public int LastVersionNumber { get; }
        public VersionComparisonResult ComparisonResult { get; }

        public VersionChangeInfo(int currentVersionNumber, int lastVersionNumber)
        {
            CurrentVersionNumber = currentVersionNumber;
            LastVersionNumber = lastVersionNumber;
            ComparisonResult = GetResult();
        }

        private VersionComparisonResult GetResult()
        {
            if (CurrentVersionNumber >= LastVersionNumber)
            {
                return CurrentVersionNumber == LastVersionNumber
                               ? VersionComparisonResult.Same
                               : VersionComparisonResult.Upgrade;
            }

            return VersionComparisonResult.Downgrade;
        }
    }
}