using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Registry;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;

namespace BreizhUp.Dishes.Starter.Cider
{
    internal class CiderStarterDish : CustomDish
    {
        public override string UniqueNameID => "CiderStarterDish";
        public override DishType Type => DishType.Starter;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.SmallDecrease;
        public override CardType CardType => CardType.Default;
        public override bool IsAvailableAsLobbyOption => true;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override int Difficulty => 1;
        public override GameObject IconPrefab => Main.bundle.LoadAsset<GameObject>("ciderbowl");
        public override GameObject DisplayPrefab => Main.bundle.LoadAsset<GameObject>("ciderbowl");
        public override List<string> StartingNameSet => new List<string>()
        {
            "Cider-ella"
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new List<Dish.MenuItem>
        {
            new Dish.MenuItem
            {
                Item = Main.CiderBowl,
                Phase = MenuPhase.Starter,
                Weight = 1,
                DynamicMenuType = DynamicMenuType.Static,
                DynamicMenuIngredient = Main.CiderBottleFull
            }
        };

        public override HashSet<Item> MinimumIngredients => new HashSet<Item>
        {
            Main.CiderBottleFull
        };

        public override HashSet<Process> RequiredProcesses => new HashSet<Process>
        {
            Main.Knead
        };

        public override Dictionary<Locale, string> Recipe => new Dictionary<Locale, string>
        {
            { Locale.English, "Lorem Ipsum" }
        };
    }
}
