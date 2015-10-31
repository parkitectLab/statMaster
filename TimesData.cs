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
