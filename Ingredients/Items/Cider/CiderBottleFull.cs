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
    internal class CiderBottleFull : CustomItem
    {
        public override string UniqueNameID => "ciderbottlefull";
        public override string ColourBlindTag => "CiBo";
        public override GameObject Prefab => Main.bundle.LoadAsset<GameObject>("ciderbottlefull");

        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.StackableFood;
        public override ItemValue ItemValue => ItemValue.Small;

        public override List<Item.ItemProcess> Processes => new List<Item.ItemProcess>
        {
            new Item.ItemProcess
            {
                Duration = 1,
                Process = Main.Knead,
                Result = Main.CiderBowl,
            }
        };
    }
}
