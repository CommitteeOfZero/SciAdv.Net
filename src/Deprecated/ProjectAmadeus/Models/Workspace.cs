using PropertyChanged;

namespace ProjectAmadeus.Models
{
    [AddINotifyPropertyChangedInterface]
    public sealed class Workspace
    {
        public bool IsEmpty => string.IsNullOrEmpty(FolderPath);

        public string FolderPath { get; set; }
        public Module CurrentModule { get; set; }
    }
}
