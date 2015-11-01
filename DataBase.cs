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
                    string content = fh.get(handle);
                    success = success && (content != null);
                    if (success) success = success && setByDict(Json.Deserialize(content) as Dictionary<string, object>);
                }
            }
            return success;
        }

        public virtual List<string> saveHandles()
        {
            errorOnSave = false;
            List<string> messages = new List<string>();
            if (!updateHandles("set"))
            {
                errorOnSave = true;
                messages.Add("Error on save handles");
            } else
            {
                messages = fh.saveAll();
                errorOnSave = fh.errorOnSave;
            }
            return messages;
        }

        public virtual List<string> loadHandles()
        {
            errorOnLoad = false;
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
