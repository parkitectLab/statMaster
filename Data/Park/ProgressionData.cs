using System;
using System.Collections.Generic;

namespace StatMaster.Data.Park
{
    class ProgressionData : BaseData
    {
        public List<uint[]> ranges = new List<uint[]>();
        public List<object> rangeObjects = new List<object>();

        public virtual void addRange()
        {
            uint[] ts = new uint[2];
            uint cTs = Main.getCurrentTimestamp();
            ts[0] = cTs;
            ts[1] = cTs;
            ranges.Add(ts);
        }

        public virtual void updateRange()
        {
            ranges[ranges.Count - 1][1] = Main.getCurrentTimestamp();
        }

        protected override Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = base.getDict(handle);

            dict.Add("ranges", ranges);

            List<object> objects = new List<object>();
            for (var idx = 0; idx < rangeObjects.Count; idx++)
            {
                objects.Add(getValuesDict(idx));
            }
            dict.Add("rangeObjects", objects);

            return dict;
        }

        protected virtual Dictionary<string, string> getValuesDict(int idx)
        {
            Dictionary<string, string> obj = new Dictionary<string, string>();
            
            return obj;
        }

        protected virtual bool addValueObj(object obj)
        {
            rangeObjects.Add(obj);
            return true;
        }

        protected override bool setObjByKey(string handle, string key, object obj)
        {
            bool success = base.setObjByKey(handle, key, obj);
            switch (key)
            {
                case "ranges":
                    List<uint[]> valuesDict = obj as List<uint[]>;
                    ranges = valuesDict;
                  break;
                case "rangeObjects":
                    List<object> objects = obj as List<object>;
                    foreach (object cObj in objects)
                        addValueObj(obj);
                    break;
            }
            return success;
        }

    }
}
