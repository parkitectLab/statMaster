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

        public void addHandle(string handle)
        {
            if (!handles.Contains(handle))
            {
                handles.Add(handle);
                fh.add(handle);
            }
        }

        public virtual void updateHandles()
        {
            foreach (string handle in handles)
            {
                fh.set(handle, Json.Serialize(getDict(handle)));
            }
        }

        public virtual List<string> saveAllHandles()
        {
            return fh.saveAll();
            
        }

        public virtual void loadHandles()
        {
            //fh.loadFiles();
            return;
        }
    }
}
