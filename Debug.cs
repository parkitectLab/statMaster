using System;
using System.Security.Cryptography;
using System.Text;

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

        public void dataNotifications(string[] names, string[] values)
        {
            for (var i = 0; i < names.Length; i++)
            {
                notification("Data { " + names[i] + " = " + values[i] + " }");
            }
        }

        public void dataNotificationsTimes(string[] names, long[] values, bool keepTime = true)
        {
            string[] newValues = new string[names.Length];
            for (var i = 0; i < names.Length; i++)
            {
                if (keepTime == true)
                {
                    newValues[i] = Convert.ToString(values[i]);
                }
                else
                {
                    TimeSpan ts = TimeSpan.FromMilliseconds(Convert.ToDouble(
                        values[i]
                    ));
                    newValues[i] = ts.ToString();
                }
            }
            dataNotifications(names, newValues);
        }

        // debug helper method to get md5 hash e.g. to check/validate file/data content status
        public string calculateMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
