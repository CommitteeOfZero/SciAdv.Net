namespace ProjectAmadeus.ViewModels
{
    public enum NotificationKind
    {
        Info,
        Error,
        UserPrompt
    }

    public abstract class Notification
    {
        public NotificationKind Kind { get; set; } = NotificationKind.Info;
        public string ConfirmText { get; set; } = "Yes";
        public string CancelText { get; set; } = "No";
    }
}
