using System.Collections.Generic;
using System;

namespace StatMaster.Data
{
    class TimeData : BaseData
    {
        // all time related values are unix timestamps
        public int sessionIdx = -1;
        public List<uint> tsSessionStarts = new List<uint>();

        // first timestamp in record to start from
        public uint tsStart = 0;
        // last timestamp in record to go to
        public uint tsEnd = 0;

        protected override bool setByDictKey(Dictionary<string, object> dict, string key)
        {
            switch (key)
            {
                case "sessionIdx":
                    sessionIdx = Convert.ToInt32(dict[key]);
                    break;
                case "tsSessionStarts":
                    List<object> starts = dict[key] as List<object>;
                    if (starts.Count > 0)
                        foreach (object start in starts) tsSessionStarts.Add(Convert.ToUInt32(start));
                    break;
                case "tsStart":
                    tsStart = Convert.ToUInt32(dict[key]);
                    break;
                case "tsEnd":
                    tsEnd = Convert.ToUInt32(dict[key]);
                    break;
            }
            return true;
        }

        protected override bool setByDict(Dictionary<string, object> dict)
        {
            bool success = base.setByDict(dict);
            sessionIdx = tsSessionStarts.Count - 1;
            return success;
        }

        protected override Dictionary<string, object> getDict(string handle)
        {
            // times data for each handle
            Dictionary<string, object> dict = base.getDict(handle);
            dict.Add("sessionIdx", sessionIdx);
            dict.Add("tsSessionStarts", tsSessionStarts);
            dict.Add("tsStart", tsStart);
            dict.Add("tsEnd", tsEnd);

            return dict;
        }
    }
}
