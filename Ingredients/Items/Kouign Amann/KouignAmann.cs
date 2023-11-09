using KitchenData;
using KitchenLib.Customs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BreizhUp.Ingredients.Items.Kouign_Amann
{
    internal class KouignAmann : CustomItem
    {
        public override string UniqueNameID => "KouignAmann";
        public override GameObject Prefab => Main.bundle.LoadAsset<GameObject>("KouignAmann");

        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.StackableFood;
        public override ItemValue ItemValue => ItemValue.SideMedium;
        public override string ColourBlindTag => "KoAm";

        public override List<Item.ItemProcess> Processes => new List<Item.ItemProcess>
        {
            new Item.ItemProcess
            {
                Duration = 5,
                Process = Main.Cook,
                Result = Main.KouignAmannBurnt,
                IsBad = true
            }
        };
    }
}
