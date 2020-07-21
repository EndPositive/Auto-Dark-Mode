using Windows.UI.Notifications;
using ADM.Views;

namespace ADM.Helpers
{
    public static class NotificationHelper
    {
        public static void New(string heading, string text)
        {
            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            
            var textNodes = template.GetElementsByTagName("text");
            if (textNodes.Length == 0) new ExceptionWindow("Could not properly initialize a notification.");
            textNodes[0].InnerText = text;

            var notifier = ToastNotificationManager.CreateToastNotifier(heading);
            var notification = new ToastNotification(template);
            notifier.Show(notification);
        }
    }
}