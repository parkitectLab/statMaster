using System.Collections.Generic;
using System;

namespace StatMaster.Data
{
    class SettingsData : BaseData
    {

        public Settings settings;

        protected override Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = base.getDict(handle);

            dict.Add("devMode", settings.devMode);

            dict.Add("updateGameData", settings.updateGameData);
            dict.Add("updateParkData", settings.updateParkData);
            dict.Add("updateParkSessionData", settings.updateParkSessionData);
            dict.Add("updateProgressionData", settings.updateProgressionData);

            dict.Add("updateFeeProgressionData", settings.updateFeeProgressionData);
            dict.Add("updateFurtherProgressionData", settings.updateFurtherProgressionData);

            dict.Add("dataUpdateInterval", settings.dataUpdateInterval);

            dict.Add("updateAutoSaveData", settings.updateAutoSaveData);
            dict.Add("ignoreQuickSaveFileNames", settings.ignoreQuickSaveFileNames);
            dict.Add("ignoreAutoSaveFileNames", settings.ignoreAutoSaveFileNames);

            return dict;
        }

        protected override bool setByDictKey(Dictionary<string, object> dict, string key)
        {
            bool success = base.setByDictKey(dict, key);

            switch (key)
            {
                case "devMode":
                    settings.devMode = Convert.ToBoolean(dict[key]);
                    break;
                case "updateGameData":
                    settings.updateGameData = Convert.ToBoolean(dict[key]);
                    break;
                case "updateParkData":
                    settings.updateParkData = Convert.ToBoolean(dict[key]);
                    break;
                case "updateParkSessionData":
                    settings.updateParkSessionData = Convert.ToBoolean(dict[key]);
                    break;
                case "updateProgressionData":
                    settings.updateProgressionData = Convert.ToBoolean(dict[key]);
                    break;
                case "updateFeeProgressionData":
                    settings.updateFeeProgressionData = Convert.ToBoolean(dict[key]);
                    break;
                case "updateFurtherProgressionData":
                    settings.updateFurtherProgressionData = Convert.ToBoolean(dict[key]);
                    break;
                case "dataUpdateInterval":
                    settings.dataUpdateInterval = Convert.ToUInt32(dict[key]);
                    break;
                case "updateAutoSaveData":
                    settings.updateAutoSaveData = Convert.ToBoolean(dict[key]);
                    break;
                case "ignoreQuickSaveFileNames":
                    settings.ignoreQuickSaveFileNames = Convert.ToBoolean(dict[key]);
                    break;
                case "ignoreAutoSaveFileNames":
                    settings.ignoreAutoSaveFileNames = Convert.ToBoolean(dict[key]);
                    break;
            }

            return true;
        }
    }
}
