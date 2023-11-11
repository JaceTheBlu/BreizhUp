using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BreizhUp.Ingredients.Providers.Cider
{
    internal class CiderBox : CustomAppliance
    {
        public override string UniqueNameID => "ciderbox";
        public override GameObject Prefab => Main.bundle.LoadAsset<GameObject>("ciderbox");

        public override bool SellOnlyAsDuplicate => true;
        public override bool IsPurchasable => true;
        public override PriceTier PriceTier => PriceTier.Medium;
        public override ShoppingTags ShoppingTags => ShoppingTags.Cooking | ShoppingTags.Misc;
        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>()
        {
            new CItemHolder()
        };
    }
}
