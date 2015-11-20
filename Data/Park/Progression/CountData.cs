using MiniJSON;
using System;
using System.Collections.Generic;

namespace StatMaster.Data.Park.Progression
{
    class ProgressionCountDataValues
    {
        public List<uint> guestsCount = new List<uint>();
        public List<uint> employeesCount = new List<uint>();
        public List<uint> attractionsCount = new List<uint>();
        public List<uint> shopsCount = new List<uint>();
    }

    class ProgressionCountData : ProgressionData
    {

        public ProgressionCountData(string parkGuid, int sessionIdx)
        {
            dataVersionIdx = 1;
            minDataVersionIdx = 1;
            setSubFolder("park_" + parkGuid + "/session_" + sessionIdx);
            addHandle("progression_counts");
        }

        public override void addRange(Settings settings)
        {
            base.addRange(settings);
            rangeObjects.Add(new ProgressionCountDataValues());
        }

        public override void updateRange()
        {
            base.updateRange();
            ProgressionCountDataValues values = (ProgressionCountDataValues)rangeObjects[ranges.Count - 1];
            values.guestsCount.Add(Convert.ToUInt32(GameController.Instance.park.getGuests().Count));
            values.employeesCount.Add(Convert.ToUInt32(GameController.Instance.park.getEmployees().Count));
            values.attractionsCount.Add(Convert.ToUInt32(GameController.Instance.park.getAttractions().Count));
            values.shopsCount.Add(Convert.ToUInt32(GameController.Instance.park.getShops().Count));
        }

        protected override Dictionary<string, string> getValuesDict(int idx)
        {
            Dictionary<string, string> obj = new Dictionary<string, string>();

            ProgressionCountDataValues values = (ProgressionCountDataValues)rangeObjects[idx];

            obj.Add("guestsCount", Json.Serialize(values.guestsCount));
            obj.Add("employeesCount", Json.Serialize(values.employeesCount));
            obj.Add("attractionsCount", Json.Serialize(values.attractionsCount));
            obj.Add("shopsCount", Json.Serialize(values.shopsCount));

            return obj;
        }

        protected override bool addValueObj(object obj)
        {
            rangeObjects.Add((ProgressionCountDataValues)obj);
            return true;
        }
    }
}
