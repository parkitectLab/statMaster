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

        public uint autoSavesCount = 0;
        public uint quickSavesCount = 0;

        // start time of session -> equivalent to ParkData.tsSessionStarts[related_idx]
        public uint tsStart = 0;
        // last updated value of ParkInfo.ParkTime in session
        public uint time = 0;

        // park progression data
        public Dictionary<uint, uint> guestsCount = new Dictionary<uint, uint>();
        public Dictionary<uint, uint> employeesCount = new Dictionary<uint, uint>();
        public Dictionary<uint, uint> attractionsCount = new Dictionary<uint, uint>();
        public Dictionary<uint, uint> shopsCount = new Dictionary<uint, uint>();

        public Dictionary<uint, float> money = new Dictionary<uint, float>();
        public Dictionary<uint, float> entranceFee = new Dictionary<uint, float>();
        public Dictionary<uint, float> ratingCleanliness = new Dictionary<uint, float>();
        public Dictionary<uint, float> ratingHappiness = new Dictionary<uint, float>();
        public Dictionary<uint, float> ratingPriceSatisfaction = new Dictionary<uint, float>();

        public Dictionary<uint, float> attractionsEntranceFeeAvg = new Dictionary<uint, float>();
        public Dictionary<uint, uint> attractionsOpenedCount = new Dictionary<uint, uint>();
        public Dictionary<uint, uint> attractionsCustomersCount = new Dictionary<uint, uint>();

        public Dictionary<uint, float> shopsProductPriceAvg = new Dictionary<uint, float>();
        public Dictionary<uint, uint> shopsOpenedCount = new Dictionary<uint, uint>();
        public Dictionary<uint, uint> shopsCustomersCount = new Dictionary<uint, uint>();

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

            dict.Add("autoSavesCount", autoSavesCount);
            dict.Add("quickSaveCount", quickSavesCount);

            dict.Add("guestsCount", guestsCount);
            dict.Add("employeesCount", employeesCount);
            dict.Add("attractionsCount", attractionsCount);
            dict.Add("shopsCount", shopsCount);

            dict.Add("money", money);
            dict.Add("entranceFee", entranceFee);
            dict.Add("ratingCleanliness", ratingCleanliness);
            dict.Add("ratingHappiness", ratingHappiness);
            dict.Add("ratingPriceSatisfaction", ratingPriceSatisfaction);

            dict.Add("attractionsEntranceFeeAvg", attractionsEntranceFeeAvg);
            dict.Add("attractionsOpenedCount", attractionsOpenedCount);
            dict.Add("attractionsCustomersCount", attractionsCustomersCount);

            dict.Add("shopsProductPriceAvg", shopsProductPriceAvg);
            dict.Add("shopsOpenedCount", shopsOpenedCount);
            dict.Add("shopsCustomersCount", shopsCustomersCount);

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
                case "autoSavesCount":
                    autoSavesCount = Convert.ToUInt32(dict[key]);
                    break;
                case "quickSavesCount":
                    quickSavesCount = Convert.ToUInt32(dict[key]);
                    break;
                case "guestsCount":
                case "employeesCount":
                case "attractionsCount":
                case "shopsCount":
                case "attractionsOpenedCount":
                case "shopsOpenedCount":
                case "attractionsCustomersCount":
                case "shopsCustomersCount":
                    Dictionary<string, object> countValuesDict = dict[key] as Dictionary<string, object>;
                    foreach (string vdKey in countValuesDict.Keys)
                    {
                        uint ts = Convert.ToUInt32(vdKey);
                        uint count = Convert.ToUInt32(countValuesDict[vdKey]);

                        switch (key)
                        {
                            case "guestsCount":
                                guestsCount.Add(ts, count);
                                break;
                            case "employeesCount":
                                employeesCount.Add(ts, count);
                                break;
                            case "attractionsCount":
                                attractionsCount.Add(ts, count);
                                break;
                            case "shopsCount":
                                shopsCount.Add(ts, count);
                                break;
                            case "attractionsOpenedCount":
                                attractionsOpenedCount.Add(ts, count);
                                break;
                            case "shopsOpenedCount":
                                shopsOpenedCount.Add(ts, count);
                                break;
                            case "attractionsCustomersCount":
                                attractionsCustomersCount.Add(ts, count);
                                break;
                            case "shopsCustomersCount":
                                shopsCustomersCount.Add(ts, count);
                                break;
                        }
                    }
                    break;
                case "money":
                case "ratingCleanliness":
                case "ratingHappiness":
                case "ratingPriceSatisfaction":
                case "entranceFee":
                case "attractionsEntranceFeeAvg":
                case "shopsProductPriceAvg":
                    Dictionary<string, object> valuesDict = dict[key] as Dictionary<string, object>;
                    foreach (string vdKey in valuesDict.Keys)
                    {
                        uint ts = Convert.ToUInt32(vdKey);
                        float value = Convert.ToSingle(valuesDict[vdKey]);

                        switch (key)
                        {
                            case "money":
                                money.Add(ts, value);
                                break;
                            case "ratingCleanliness":
                                ratingCleanliness.Add(ts, value);
                                break;
                            case "ratingHappiness":
                                ratingHappiness.Add(ts, value);
                                break;
                            case "ratingPriceSatisfaction":
                                ratingPriceSatisfaction.Add(ts, value);
                                break;
                            case "entranceFee":
                                entranceFee.Add(ts, value);
                                break;
                            case "attractionsEntranceFeeAvg":
                                attractionsEntranceFeeAvg.Add(ts, value);
                                break;
                            case "shopsProductPriceAvg":
                                shopsProductPriceAvg.Add(ts, value);
                                break;
                        }
                    }
                    break;
            }
            return success;
        }
    }
}
