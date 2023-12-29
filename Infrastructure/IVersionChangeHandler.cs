namespace MbsCore.VersionLeapSync
{
    public interface IVersionChangeHandler
    {
        VersionComparer VersionComparer { get; }

        void Execute(IVersionChangeInfo info);
    }
}