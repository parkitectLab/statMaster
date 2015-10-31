using System.Collections.Generic;
using MiniJSON;

namespace StatMaster
{
    class DataBase
    {
        protected FilesHandler fh = new FilesHandler();
        protected List<string> handles = new List<string>();

        protected virtual Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            return dict;
        }

        protected virtual bool setByDict(Dictionary<string, object> dict)
        {
            return true;
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
                    if (success) setByDict(Json.Deserialize(content) as Dictionary<string, object>);
                }
            }
            return success;
        }

        public virtual List<string> saveHandles()
        {
            if (updateHandles("set")) return fh.saveAll();
            return null;
            
        }

        public virtual List<string> loadHandles()
        {
            List<string> messages = fh.loadAll();
            if (updateHandles("get")) return messages;
            return null;
        }
    }
}
