using KitchenData;
using KitchenLib.Customs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BreizhUp.Ingredients.Items.Butter
{
    internal class Butter : CustomItem
    {
        public override string UniqueNameID => "Butter";
        public override GameObject Prefab => Main.bundle.LoadAsset<GameObject>("Butter");

        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.StackableFood;
        public override ItemValue ItemValue => ItemValue.SideMedium;
        public override string ColourBlindTag => "Bu";
    }
}
