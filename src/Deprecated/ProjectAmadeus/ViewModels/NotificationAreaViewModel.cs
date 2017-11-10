using Caliburn.Micro;
using ProjectAmadeus.ViewModels.Notifications;
using System.Threading.Tasks;

namespace ProjectAmadeus.ViewModels
{
    public sealed class NotificationAreaViewModel : Screen
    {
        private TaskCompletionSource<bool> _interaction;

        public NotificationAreaViewModel()
        {
            if (Execute.InDesignMode)
            {
                IsVisible = true;
                CurrentNotification = new GenericNotificationViewModel(string.Empty, NotificationKind.Info);
            }
        }

        public bool IsVisible { get; private set; }
        public Notification CurrentNotification { get; private set; }

        public void ShowNotification(Notification notification)
        {
            _interaction = new TaskCompletionSource<bool>();
            CurrentNotification = notification;
            IsVisible = true;
        }

        public Task<bool> ShowNotificationAsync(Notification notification)
        {
            ShowNotification(notification);
            return _interaction.Task;
        }

        public void DismissNotification()
        {
            Hide();
            _interaction.SetResult(true);
        }

        public void Confirm()
        {
            Hide();
            _interaction.SetResult(true);
        }

        public void Cancel()
        {
            Hide();
            _interaction.SetResult(false);
        }

        private void Hide() => IsVisible = false;
    }
}
