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

            dict.Add("updateParkProgressionData", settings.updateParkProgressionData);
            dict.Add("updateFurtherParkProgressionData", settings.updateFurtherParkProgressionData);

            dict.Add("updatePeopleProgressionData", settings.updatePeopleProgressionData);

            dict.Add("updateAttractionsProgressionData", settings.updateAttractionsProgressionData);
            dict.Add("updateFurtherAttractionsProgressionData", settings.updateFurtherAttractionsProgressionData);

            dict.Add("updateShopsProgressionData", settings.updateShopsProgressionData);
            dict.Add("updateFurtherShopsProgressionData", settings.updateFurtherShopsProgressionData);

            dict.Add("progressionDataUpdateInterval", settings.progressionDataUpdateInterval);

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
                case "updateParkProgressionData":
                    settings.updateParkProgressionData = Convert.ToBoolean(dict[key]);
                    break;
                case "updateFurtherParkProgressionData":
                    settings.updateFurtherParkProgressionData = Convert.ToBoolean(dict[key]);
                    break;
                case "updatePeopleProgressionData":
                    settings.updatePeopleProgressionData = Convert.ToBoolean(dict[key]);
                    break;
                case "updateAttractionsProgressionData":
                    settings.updateAttractionsProgressionData = Convert.ToBoolean(dict[key]);
                    break;
                case "updateFurtherAttractionsProgressionData":
                    settings.updateFurtherAttractionsProgressionData = Convert.ToBoolean(dict[key]);
                    break;
                case "updateShopsProgressionData":
                    settings.updateShopsProgressionData = Convert.ToBoolean(dict[key]);
                    break;
                case "updateFurtherShopsProgressionData":
                    settings.updateFurtherShopsProgressionData = Convert.ToBoolean(dict[key]);
                    break;
                case "progressionDataUpdateInterval":
                    settings.progressionDataUpdateInterval = Convert.ToUInt32(dict[key]);
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
