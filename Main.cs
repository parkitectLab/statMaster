using UnityEngine;

namespace StatMaster
{
    class Main : IMod
    {
        private GameObject _go;

        public void onEnabled()
        {
            _go = new GameObject();
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
    }
}
