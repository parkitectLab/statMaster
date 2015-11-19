using System.Collections.Generic;
using MiniJSON;
using System;

namespace StatMaster.Data
{
    class BaseData
    {
        public uint dataVersionIdx = 0;
        public uint minDataVersionIdx = 0;
        public bool invalidDataVersion = false;

        protected FilesHandler fh = new FilesHandler();
        protected List<string> handles = new List<string>();

        public bool errorOnLoad = false;
        public bool errorOnSave = false;

        // get dict to save data to json
        protected virtual Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("dataVersionIdx", dataVersionIdx);
            return dict;
        }

        // set object by key to properties
        protected virtual bool setObjByKey(string handle, string key, object obj)
        {
            bool success = true;
            switch (key)
            {
                case "dataVersionIdx":
                    dataVersionIdx = Convert.ToUInt32(obj);
                    break;
            }
            return success;
        }

        // set by dict to get data from json data related dictionary
        protected virtual bool setByDict(string handle, Dictionary<string, object> dict)
        {
            bool success = true;
            foreach (string key in dict.Keys)
            {
                success = success && setObjByKey(handle, key, dict[key]);
            }
            return success;
        }

        public void setSubFolder(string subFolder)
        {
            fh.setSubFolder(subFolder);
        }

        public void addHandle(string handle)
        {
            if (!handles.Contains(handle))
            {
                handles.Add(handle);
                fh.add(handle);
            }
        }

        public virtual bool updateByHandles(string mode = "set")
        {
            bool success = true;
            foreach (string handle in handles)
            {
                if (mode == "set")
                {
                    Dictionary<string, object> dict = getDict(handle);
                    success = success && (dict != null);
                    if (success) success = success && fh.set(handle, Json.Serialize(dict));
                }
                else
                {
                    string content = fh.get(handle);
                    success = success && (content != null);
                    if (success) success = success && setByDict(
                        handle, Json.Deserialize(content) as Dictionary<string, object>);
                    if (minDataVersionIdx > 0)
                    {
                        if (dataVersionIdx != minDataVersionIdx)
                        {
                            fh.delete(handle);
                            invalidDataVersion = true;
                        }
                    }

                }
            }
            return success;
        }

        public virtual List<string> saveByHandles()
        {
            errorOnSave = false;
            errorOnSave = !updateByHandles("set");
            List<string> messages = new List<string>();
            if (!errorOnSave) {
                messages = fh.saveAll();
                errorOnSave = fh.errorOnSave;
            }
            if (errorOnSave) messages.Add("Error on save handles"); 
            return messages;
        }

        public virtual List<string> loadByHandles()
        {
            errorOnLoad = false;
            List<string> messages = fh.loadAll();
            errorOnLoad = fh.errorOnLoad;
            if (!errorOnLoad && !updateByHandles("get")) errorOnLoad = true;
            if (errorOnLoad) messages.Add("Error on load handles"); 
            return messages;
        }
    }
}
