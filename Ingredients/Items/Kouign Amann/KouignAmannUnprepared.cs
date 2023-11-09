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
using static Kitchen.ItemGroupView;
using static KitchenData.ItemGroup;

namespace BreizhUp.Ingredients.Items.Kouign_Amann
{
    internal class KouignAmannUnprepared : CustomItemGroup<KouignAmannUnpreparedItemGroupView>
    {
        public override string UniqueNameID => "KouignAmannUnprepared";
        public override GameObject Prefab => Main.bundle.LoadAsset<GameObject>("KouignAmannUnprepared");
        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.StackableFood;
        public override ItemValue ItemValue => ItemValue.Medium;
        public override List<ItemSet> Sets => new()
        {
            new ItemSet()
            {
                Max = 3,
                Min = 3,
                IsMandatory = true,
                Items = new List<Item>()
                {
                    Main.Dough,
                    Main.Butter,
                    Main.Sugar
                }
            }
        };
        public override List<Item.ItemProcess> Processes => new()
        {
            new Item.ItemProcess()
            {
                Duration = 20f,
                Process = Main.Knead,
                Result = Main.KouignAmannRaw
            }
        };
        public override void OnRegister(GameDataObject gameDataObject)
        {
            var materials = new Material[]
            {
                MaterialUtils.GetExistingMaterial("Bread - Inside"),
            };
            MaterialUtils.ApplyMaterial(Prefab, "GameObject", materials);
            materials[0] = MaterialUtils.GetExistingMaterial("Bread - Cooked");
            MaterialUtils.ApplyMaterial(Prefab, "GameObject (1)", materials);
            materials[0] = MaterialUtils.GetExistingMaterial("Tomato");
            MaterialUtils.ApplyMaterial(Prefab, "Tomato", materials);
            materials[0] = MaterialUtils.GetExistingMaterial("Olive Oil Bottle");
            MaterialUtils.ApplyMaterial(Prefab, "GameObject (3)", materials);
            materials[0] = MaterialUtils.GetExistingMaterial("Onion - Cooked");
            MaterialUtils.ApplyMaterial(Prefab, "Onion", materials);

            Prefab.GetComponent<KouignAmannUnpreparedItemGroupView>()?.Setup(Prefab);
        }
    }
    public class KouignAmannUnpreparedItemGroupView : ItemGroupView
    {
        internal void Setup(GameObject prefab)
        {
            // This tells which sub-object of the prefab corresponds to each component of the ItemGroup
            // All of these sub-objects are hidden unless the item is present
            ComponentGroups = new()
            {
                new()
                {
                    GameObject = GameObjectUtils.GetChildObject(prefab, "Dough"),
                    Item = Main.Dough
                },
                new()
                {
                    GameObject = GameObjectUtils.GetChildObject(prefab, "Butter"),
                    Item = Main.Butter
                },
                new()
                {
                    GameObject = GameObjectUtils.GetChildObject(prefab, "Sugar"),
                    Item = Main.Sugar
                }
            };

            ComponentLabels = new()
            {
                new()
                {
                    Text = "Do",
                    Item = Main.Dough
                },
                new()
                {
                    Text = "Bu",
                    Item = Main.Butter
                },
                new()
                {
                    Text = "Su",
                    Item = Main.Sugar
                }
            };
        }
    }
}
