using KitchenData;
using KitchenLib;
using KitchenLib.Logging;
using KitchenLib.References;
using KitchenMods;
using System.Reflection;
using KitchenLib.Utils;
using UnityEngine;

using System.Linq;
using System.IO;
using IngredientLib.Ingredient.Items;

namespace BreizhUp
{
    public class Main : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.hungrydevs.breizhup";
        public const string MOD_NAME = "BreizhUp";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "HungryDevs";
        public const string MOD_GAMEVERSION = ">=1.1.4";

        public static AssetBundle bundle;
        public static KitchenLogger Logger;

        // Vanilla Processes
        internal static Process Cook => (Process)GDOUtils.GetExistingGDO(ProcessReferences.Cook);
        internal static Process Knead => (Process)GDOUtils.GetExistingGDO(ProcessReferences.Knead);

        // Vanilla Items
        internal static Item Flour => (Item)GDOUtils.GetExistingGDO(ItemReferences.Flour);
        internal static Item Sugar => (Item)GDOUtils.GetExistingGDO(ItemReferences.Sugar);
        internal static Item Dough => (Item)GDOUtils.GetExistingGDO(ItemReferences.Dough);

        // Modded Items
        internal static Item Butter => (Item)GDOUtils.GetCustomGameDataObject<Butter>().GameDataObject;

        // Kouign Amann

        public Main() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly())
        {
            string bundlePath = Path.Combine(new string[] { Directory.GetParent(Application.dataPath).FullName, "Mods", ModID });

            Debug.Log($"{MOD_NAME} {MOD_VERSION} {MOD_AUTHOR}: Loaded");
            Debug.Log($"Assets Loaded From {bundlePath}");
        }

        protected override void OnInitialise()
        {
            Logger.LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            Logger = InitLogger();
            bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).ToList()[0];

            // Dishes
            // Desserts
        }
    }
}

