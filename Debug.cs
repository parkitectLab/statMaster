#if !UNITY_EDITOR
#define DEBUG_LOG_OVERWRAP
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

#if DEBUG_LOG_OVERWRAP
public static class Debug
{

    static public bool isEnable = true;

    static public bool consoleOnly = true;

    static private string textPrefix = "StatMaster";

    static private bool showTime = true;

    static public void Break()
    {
        if (IsEnable())
        {
            UnityEngine.Debug.Break();
        }
    }

    static public void Log(object message)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.Log(message);
        }
    }

    static public void Log(object message, UnityEngine.Object context)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.Log(message, context);
        }
    }

    static public void LogWarning(object message)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.LogWarning(message);
        }
    }

    static public void LogWarning(object message, UnityEngine.Object context)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.LogWarning(message, context);
        }
    }

    static public void LogError(object message)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.LogError(message);
        }
    }

    static public void LogError(object message, UnityEngine.Object context)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.LogError(message, context);
        }
    }

    static public void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.0F, bool depthTest = true)
    {
        UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    }

    static public void LogMT(string text)
    {
        if (IsEnable())
        {
            text = textPrefix + ((showTime) ? " " + DateTime.Now.ToString("HH:mm:ss") : "") + ": " + text;

            if (consoleOnly)
            {
                UnityEngine.Debug.Log(text);
            }
            else
            {
                Parkitect.UI.NotificationBar.Instance.addNotification(text);
            }
        }
    }

    static public void LogMT(List<string> texts)
    {
        foreach (string text in texts) LogMT(text);
    }

    static public void LogMT(string text, uint timestamp)
    {
        text = text + " " + Convert.ToString(getDateTime(timestamp));
        LogMT(text);
    }

    static public void LogMT(string[] names, string[] values)
    {
        for (var i = 0; i < names.Length; i++)
        {
            LogMT("Data { " + names[i] + " = " + values[i] + " }");
        }
    }

    static public void LogMT(string[] names, long[] values, uint mode = 0)
    {
        string[] newValues = new string[names.Length];
        for (var i = 0; i < names.Length; i++)
        {
            if (mode == 0)
            {
                newValues[i] = values[i].ToString();
            }
            else if (mode == 1)
            {
                newValues[i] = getDateTime(Convert.ToUInt32(values[i])).ToString();
            }
            else if (mode == 2)
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(Convert.ToDouble(
                    values[i]
                ));
                newValues[i] = ts.ToString();

            }
        }
        LogMT(names, newValues);
    }

    static public DateTime getDateTime(uint timestamp)
    {
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return dtDateTime.AddSeconds(Convert.ToDouble(timestamp)).ToLocalTime();
    }

    static bool IsEnable()
    {
        return isEnable;
    }
}
#endif