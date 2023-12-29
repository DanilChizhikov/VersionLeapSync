namespace MbsCore.VersionLeapSync
{
    public interface IVersionSaveAdapter
    {
        string Load(string defaultValue);
        void Save(string value);
    }
}