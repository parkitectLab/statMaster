using System.Collections.Generic;

namespace StatMaster
{
    class ParkSessionData : DataBase
    {
        public int idx = -1;
        // to recognize name changes
        public List<string> names = new List<string>();
        // to recognize save file changes
        public List<string> saveFiles = new List<string>();

        // start time of session -> equivalent to ParkData.tsSessionStarts[related_idx]
        public uint tsStart = 0;
        // last updated value of ParkInfo.ParkTime in session
        public uint time = 0;

        protected override Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = base.getDict(handle);
            dict.Add("idx", idx);
            dict.Add("tsStart", tsStart);
            dict.Add("time", time);

            dict.Add("names", names);
            dict.Add("saveFiles", saveFiles);

            return dict;
        }

        protected override bool setByDict(Dictionary<string, object> dict)
        {
            bool success = base.setByDict(dict);
            foreach (string key in dict.Keys)
            {
                switch (key)
                {
                    case "idx":
                        idx = (int)dict[key];
                        break;
                    case "tsStart":
                        tsStart = (uint)dict[key];
                        break;
                    case "time":
                        time = (uint)dict[key];
                        break;
                    case "names":
                        List<object> dNames = (List<object>)dict[key];
                        foreach (object name in dNames) names.Add(name.ToString());
                        break;
                    case "saveFiles":
                        List<object> dSaveFiles = (List<object>)dict[key];
                        foreach (object saveFile in dSaveFiles) saveFiles.Add(saveFile.ToString());
                        break;
                }
            }
            return success;
        }

    }
}
