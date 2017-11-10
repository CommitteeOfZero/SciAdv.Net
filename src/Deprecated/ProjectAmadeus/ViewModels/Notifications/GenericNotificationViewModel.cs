namespace ProjectAmadeus.ViewModels.Notifications
{
    public sealed class GenericNotificationViewModel : Notification
    {
        public GenericNotificationViewModel(string message, NotificationKind kind)
        {
            Message = message;
            Kind = kind;
        }

        public GenericNotificationViewModel()
        {
            Message = "Design Time Message";
        }

        public string Message { get; }
    }
}
