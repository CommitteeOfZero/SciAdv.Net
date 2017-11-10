using Caliburn.Micro;

namespace ProjectAmadeus.ViewModels.Notifications
{
    public sealed class SaveChangesPromptViewModel : Notification
    {
        public SaveChangesPromptViewModel(string fileName)
        {
            FileName = fileName;
            Kind = NotificationKind.UserPrompt;
        }

        public SaveChangesPromptViewModel()
        {
            if (Execute.InDesignMode)
            {
                FileName = "test.scx";
            }
        }

        public string FileName { get; set; }
    }
}
