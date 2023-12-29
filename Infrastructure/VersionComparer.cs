namespace MbsCore.VersionLeapSync
{
    public enum VersionComparer : byte
    {
        None = 0,
        NotEqual = 1,
        Greater = 2,
        Less = 3,
        Always = 4,
    }
}