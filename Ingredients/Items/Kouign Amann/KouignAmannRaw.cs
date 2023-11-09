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
    internal class KouignAmannRaw : CustomItem
    {
        public override string UniqueNameID => "KouignAmannRaw";
        public override GameObject Prefab => Main.bundle.LoadAsset<GameObject>("KouignAmannRaw");

        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.StackableFood;
        public override ItemValue ItemValue => ItemValue.Medium;
        public override List<Item.ItemProcess> Processes => new List<Item.ItemProcess>
        {
            new Item.ItemProcess
            {
                Duration = 5,
                Process = Main.Cook,
                Result = Main.KouignAmann,
            }
        };
    }
}
