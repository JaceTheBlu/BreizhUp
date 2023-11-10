using KitchenData;
using KitchenLib.Customs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BreizhUp.Ingredients.Items.Cider
{
    internal class CiderBowl : CustomItem
    {
        public override string UniqueNameID => "CiderBowl";
        public override string ColourBlindTag => "CiBr";
        public override GameObject Prefab => Main.bundle.LoadAsset<GameObject>("ciderbowl");

        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.None;
        public override ItemValue ItemValue => ItemValue.SideMedium;
        public override bool IsConsumedByCustomer => true;

    }
}
