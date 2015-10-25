using System;
using System.Reflection;

namespace StatMaster
{
    class Debug
    {
        private string textPrefix = "StatMasterDebug: "; 

        public bool outputActive = true;

        public void notification(string text)
        {
            if (outputActive == true)
            {
                Parkitect.UI.NotificationBar.Instance.addNotification(textPrefix + text);
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
