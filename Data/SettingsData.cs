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
            dict.Add("progressionDataUpdateInterval", settings.progressionDataUpdateInterval);

            dict.Add("updatePeopleData", settings.updatePeopleData);
            dict.Add("updateAttractionsData", settings.updateAttractionsData);
            dict.Add("updateShopsnData", settings.updateShopsData);

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
                case "progressionDataUpdateInterval":
                    settings.progressionDataUpdateInterval = Convert.ToUInt32(dict[key]);
                    break;
                case "updatePeopleData":
                    settings.updatePeopleData = Convert.ToBoolean(dict[key]);
                    break;
                case "updateAttractionsData":
                    settings.updateAttractionsData = Convert.ToBoolean(dict[key]);
                    break;
                case "updateShopsData":
                    settings.updateShopsData = Convert.ToBoolean(dict[key]);
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
