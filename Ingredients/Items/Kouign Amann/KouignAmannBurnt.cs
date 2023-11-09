using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BreizhUp.Ingredients.Items.Kouign_Amann
{
    internal class KouignAmannBurnt : CustomItem
    {
        public override string UniqueNameID => "KouignAmannBurnt";
        public override GameObject Prefab => Main.bundle.LoadAsset<GameObject>("KouignAmannBurnt");
        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.OutsideRubbish;

        public override void OnRegister(GameDataObject gameDataObject)
        {
            var materials = new Material[]
            {
                MaterialUtils.GetExistingMaterial("Burned"),
             };
            MaterialUtils.ApplyMaterial(Prefab, "GameObject", materials);
            materials[0] = MaterialUtils.GetExistingMaterial("Burned - Light");
            MaterialUtils.ApplyMaterial(Prefab, "GameObject (1)", materials);
        }
    }
}
