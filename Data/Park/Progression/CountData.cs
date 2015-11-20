using System;
using MiniJSON;
using System.Collections.Generic;
using System.Reflection;

namespace StatMaster.Data.Park.Progression
{
    class ParkCountDataValues
    {
        public List<uint> guestsCount = new List<uint>();
        public List<uint> employeesCount = new List<uint>();
        public List<uint> attractionsCount = new List<uint>();
        public List<uint> shopsCount = new List<uint>();
    }

    class ParkCountData : ProgressionData
    {

        public ParkCountData(string parkGuid, int sessionIdx)
        {
            dataVersionIdx = 1;
            minDataVersionIdx = 1;
            setSubFolder("park_" + parkGuid + "/session_" + sessionIdx);
            addHandle("counts");
        }

        public override void addRange()
        {
            base.addRange();
            rangeObjects.Add(new ParkCountDataValues());
        }

        public void updateRange(uint guestsCount, uint employeesCount, uint attractionsCount, uint shopsCount)
        {
            base.updateRange();
            ParkCountDataValues values = (ParkCountDataValues)rangeObjects[ranges.Count - 1];
            values.guestsCount.Add(guestsCount);
            values.employeesCount.Add(guestsCount);
            values.attractionsCount.Add(guestsCount);
            values.shopsCount.Add(guestsCount);
        }

        protected override Dictionary<string, string> getValuesDict(int idx)
        {
            Dictionary<string, string> obj = new Dictionary<string, string>();

            ParkCountDataValues values = (ParkCountDataValues)rangeObjects[idx];

            obj.Add("guestsCount", Json.Serialize(values.guestsCount));
            obj.Add("employeesCount", Json.Serialize(values.employeesCount));
            obj.Add("attractionsCount", Json.Serialize(values.attractionsCount));
            obj.Add("shopsCount", Json.Serialize(values.shopsCount));

            return obj;
        }

        protected override bool addValueObj(object obj)
        {
            rangeObjects.Add((ParkCountDataValues)obj);
            return true;
        }
    }
}
