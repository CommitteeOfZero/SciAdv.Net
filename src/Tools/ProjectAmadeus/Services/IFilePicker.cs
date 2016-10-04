namespace ProjectAmadeus.Services
{
    public interface IFilePicker
    {
        string PickOpen(string extensions, string description);
        string PickSave(string extensions, string description);
    }
}
