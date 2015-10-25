
using UnityEngine;
using System;

namespace StatMaster
{
    [Serializable]
    class Data : System.Object
    {
        public string file = Application.persistentDataPath + "/statMaster.dat";

        public long totalRunTime = 0;
        public long currentRunTime = 0;

        public void print(Behaviour behaviour)
        {
            TimeSpan tsCurrentRunTime = TimeSpan.FromMilliseconds(Convert.ToDouble(currentRunTime));
            TimeSpan tsTotalRunTime = TimeSpan.FromMilliseconds(Convert.ToDouble(totalRunTime));

            string[] names = { "currentRunTime", "totalRunTime" };
            string[] values = { tsCurrentRunTime.ToString(), tsTotalRunTime.ToString() };

            for (int i = 0; i < values.Length; i++)
            {
                behaviour.showDebugNotfication("Data { " + names[i] + " = " + values[i] + " }");
            }
        }
    }
}
