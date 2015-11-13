using System;
using System.Collections.Generic;

namespace StatMaster.Data
{
    class ParkSessionData : BaseData
    {
        public int idx = -1;
        // to recognize name changes
        public List<string> names = new List<string>();
        // to recognize save file changes
        public List<string> saveFiles = new List<string>();
        // to recognize a file load
        public string loadFile = "";

        // start time of session -> equivalent to ParkData.tsSessionStarts[related_idx]
        public uint tsStart = 0;
        // last updated value of ParkInfo.ParkTime in session
        public uint time = 0;

        protected override Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = base.getDict(handle);
            if (idx == -1) return null;

            dict.Add("idx", idx);
            dict.Add("tsStart", tsStart);
            dict.Add("time", time);

            dict.Add("names", names);
            dict.Add("saveFiles", saveFiles);
            dict.Add("loadFile", loadFile);

            return dict;
        }

        protected override bool setByDictKey(Dictionary<string, object> dict, string key)
        {
            bool success = base.setByDictKey(dict, key);
            switch (key)
            {
                case "idx":
                    idx = Convert.ToInt32(dict[key]);
                    break;
                case "tsStart":
                    tsStart = Convert.ToUInt32(dict[key]);
                    break;
                case "time":
                    time = Convert.ToUInt32(dict[key]);
                    break;
                case "names":
                    List<object> dNames = dict[key] as List<object>;
                    if (dNames.Count > 0)
                        foreach (object name in dNames) names.Add(name.ToString());
                    break;
                case "saveFiles":
                    List<object> dSaveFiles = dict[key] as List<object>;
                    if (dSaveFiles.Count > 0)
                        foreach (object saveFile in dSaveFiles) saveFiles.Add(saveFile.ToString());
                    break;
                case "loadFile":
                    loadFile = (dict[key].ToString() != null) ? dict[key].ToString() : "";
                    break;
            }
            return success;
        }
    }
}
