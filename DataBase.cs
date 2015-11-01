using System.Collections.Generic;
using MiniJSON;

namespace StatMaster
{
    class DataBase
    {
        protected FilesHandler fh = new FilesHandler();
        protected List<string> handles = new List<string>();

        protected bool errorOnLoad = false;
        protected bool errorOnSave = false;

        protected virtual Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            return dict;
        }

        protected virtual bool setByDictKey(Dictionary<string, object> dict, string key)
        {
            return true;
        }

        protected virtual bool setByDict(Dictionary<string, object> dict)
        {
            bool success = true;
            foreach (string key in dict.Keys)
            {
                success = success && setByDictKey(dict, key);
            }
            UnityEngine.Debug.Log(success);
            return success;
        }

        public void addHandle(string handle)
        {
            if (!handles.Contains(handle))
            {
                handles.Add(handle);
                fh.add(handle);
            }
        }

        public virtual bool updateHandles(string mode = "set")
        {
            bool success = true;
            foreach (string handle in handles)
            {
                if (mode == "set")
                {
                    success = success && fh.set(handle, Json.Serialize(getDict(handle)));
                }
                else
                {
                    UnityEngine.Debug.Log("update handles mode get for " + handle);
                    string content = fh.get(handle);
                    success = success && (content != null);
                    if (success) success = success && setByDict(Json.Deserialize(content) as Dictionary<string, object>);
                }
            }

            UnityEngine.Debug.Log(success);

            return success;
        }

        public virtual List<string> saveHandles()
        {
            errorOnSave = false;
            List<string> messages = fh.saveAll();
            if (!fh.errorOnSave && !updateHandles("set"))
            {
                messages.Add("Error on save handles");
                errorOnSave = true;
            }
            return messages;
        }

        public virtual List<string> loadHandles()
        {
            errorOnLoad = false;
            UnityEngine.Debug.Log("Load handles here ...");
            List<string> messages = fh.loadAll();
            if (!fh.errorOnLoad && !updateHandles("get"))
            {
                messages.Add("Error on load handles");
                errorOnLoad = true;
            }
            return messages;
        }
    }
}
