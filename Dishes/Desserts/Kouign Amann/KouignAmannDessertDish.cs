using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KitchenData;
using KitchenLib.References;
using KitchenLib;
using KitchenLib.Utils;
using UnityEngine;
using KitchenLib.Customs;

namespace BreizhUp.Dishes.Desserts.Kouign_Amann
{
    internal class KouignAmannDessertDish : CustomDish
    {
        public override string UniqueNameID => "Kouign Amann Dish";
        public override DishType Type => DishType.Dessert;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.SmallDecrease;
        public override CardType CardType => CardType.Default;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override List<Dish.MenuItem> ResultingMenuItems => new List<Dish.MenuItem>
        {
            new Dish.MenuItem
            {
                Item = Main.KouignAmann,
                Phase = MenuPhase.Dessert,
                Weight = 1
            }
        };

        public override HashSet<Item> MinimumIngredients => new HashSet<Item>
        {
            Main.Flour,
            Main.Butter,
            Main.Sugar        
        };

        public override HashSet<Process> RequiredProcesses => new HashSet<Process>
        {
            Main.Cook,
            Main.Knead
        };

        public override Dictionary<Locale, string> Recipe => new Dictionary<Locale, string>
        {
            { Locale.English, "Lorem Ipsum" }
        };
    }
}
