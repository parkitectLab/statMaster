using System;

namespace StatMaster
{
    class Debug
    {
        private string textPrefix = "StatMaster";

        private bool showTime = true;

        public bool outputActive = true;

        public void notification(string text)
        {
            if (outputActive == true)
            {
                text = textPrefix + ((showTime) ? " " + DateTime.Now.ToString("HH:mm:ss") : "") + ": " + text;
                Parkitect.UI.NotificationBar.Instance.addNotification(text);
            }
        }

        public void dataNotifications(string[] names, long[] values)
        {
            for (var i = 0; i < names.Length; i++)
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(Convert.ToDouble(
                    values[i]
                ));
                notification("Data { " + names[i] + " = " + ts.ToString() + " }");
            }
        }
    }
}
