namespace ProjectAmadeus
{
    public interface IEditor
    {
        bool AnyUnsavedChanges { get; }
        void SaveChanges();
    }
}
