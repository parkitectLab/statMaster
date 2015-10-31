using System.Collections.Generic;

namespace StatMaster
{
    class TimesData : DataBase
    {
        // all time related values are unix timestamps
        public int sessionIdx = -1;
        public List<uint> tsSessionStarts = new List<uint>();

        // first timestamp in record to start from
        public uint tsStart = 0;
        // last timestamp in record to go to
        public uint tsEnd = 0;

        protected override bool setByDict(Dictionary<string, object> dict)
        {
            bool success = base.setByDict(dict);
            foreach (string key in dict.Keys)
            {
                switch (key)
                {
                    case "sessionIdx":
                        sessionIdx = (int)dict[key];
                        break;
                    case "tsSessionStarts":
                        List<object> starts = (List<object>)dict[key];
                        foreach (object start in starts) tsSessionStarts.Add((uint)start);
                        break;
                    case "tsStart":
                        tsStart = (uint)dict[key];
                        break;
                    case "tsEnd":
                        tsEnd = (uint)dict[key];
                        break;
                }
            }
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
