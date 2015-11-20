using MiniJSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StatMaster.Data.Park.Progression
{
    class ProgressionShopsDataValues
    {
        public List<float> productPriceAvg = new List<float>();
        public List<uint> openedCount = new List<uint>();
        public List<uint> customersCount = new List<uint>();
    }

    class ProgressionShopsData : ProgressionData
    {

        public ProgressionShopsData(string parkGuid, int sessionIdx)
        {
            dataVersionIdx = 1;
            minDataVersionIdx = 1;
            setSubFolder("park_" + parkGuid + "/session_" + sessionIdx);
            addHandle("progression_shops");
        }

        public override void addRange(Settings settings)
        {
            base.addRange(settings);
            rangeObjects.Add(new ProgressionShopsDataValues());
        }

        public override void updateRange()
        {
            base.updateRange();

            float productPriceAvg = 0f;
            ReadOnlyCollection<Shop> shops = GameController.Instance.park.getShops();
            ProductShop ps;
            ProductShopSettings pss;
            float currentPriceAvg;
            for (int i = 0; i < shops.Count; i++)
            {
                ps = (ProductShop)shops[i];
                pss = (ProductShopSettings)ps.getSettings();
                currentPriceAvg = 0f;
                for (int j = 0; j < ps.products.Length; j++)
                {
                    currentPriceAvg += pss.getProductSettings(ps.products[j]).price;
                }
                if (currentPriceAvg > 0f)
                    currentPriceAvg = currentPriceAvg / ps.products.Length;
                productPriceAvg += currentPriceAvg;
            }
            if (productPriceAvg > 0f)
                productPriceAvg = productPriceAvg / shops.Count;

            // todo: improvements to get correct relations 
            // to changes in shops builded / destroyed status progression
            // related events available? need more info
            uint openedCount = 0;
            uint customersCount = 0;
            for (int i = 0; i < shops.Count; i++)
            {
                if (shops[i].opened) openedCount++;
                customersCount = Convert.ToUInt32(shops[i].customersCount);
            }

            ProgressionShopsDataValues values = (ProgressionShopsDataValues)rangeObjects[ranges.Count - 1];
            values.productPriceAvg.Add(productPriceAvg);
            values.openedCount.Add(openedCount);
            values.customersCount.Add(customersCount);
        }

        protected override Dictionary<string, string> getValuesDict(int idx)
        {
            Dictionary<string, string> obj = new Dictionary<string, string>();

            ProgressionShopsDataValues values = (ProgressionShopsDataValues)rangeObjects[idx];

            obj.Add("productPriceAvg", Json.Serialize(values.productPriceAvg));
            obj.Add("openedCount", Json.Serialize(values.openedCount));
            obj.Add("customersCount", Json.Serialize(values.customersCount));

            return obj;
        }

        protected override bool addValueObj(object obj)
        {
            rangeObjects.Add((ProgressionShopsDataValues)obj);
            return true;
        }
    }
}
