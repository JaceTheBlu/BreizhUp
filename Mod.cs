using KitchenLib;
using KitchenLib.Logging;
using KitchenMods;
using System.Reflection;
using UnityEngine;

namespace BreizhUp
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.hungrydevs.breizhup";
        public const string MOD_NAME = "BreizhUp";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "HungryDevs";
        public const string MOD_GAMEVERSION = ">=1.1.4";

        public static KitchenLogger Logger;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

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
        }
    }
}
