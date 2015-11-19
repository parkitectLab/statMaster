using System;
using UnityEngine;

namespace StatMaster
{
    public class Main : IMod
    {
        private GameObject _go;

        public void onEnabled()
        {
            _go = new GameObject();
            _go.AddComponent<Settings>();
            _go.AddComponent<Behaviour>();
        }

        public void onDisabled()
        {
            UnityEngine.Object.Destroy(_go);
        }

        public string Name
        {
            get { return "Stat Master"; }
        }

        public string Description
        {
            get { return "All you need to get your stats mastered!"; }
        }

        static public uint getCurrentTimestamp()
        {
            TimeSpan epochTicks = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
            TimeSpan unixTicks = new TimeSpan(DateTime.UtcNow.Ticks) - epochTicks;
            return Convert.ToUInt32(unixTicks.TotalSeconds);
        }
    }
}
