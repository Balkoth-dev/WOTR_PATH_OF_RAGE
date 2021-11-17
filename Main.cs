using BlueprintCore.Blueprints;
using HarmonyLib;
using Kingmaker.Blueprints;
using System.Linq;
using UnityModManagerNet;
using WOTR_PATH_OF_RAGE.Utilities;

namespace WOTR_PATH_OF_RAGE
{
    public class Main
    {
        public static UnityModManager.ModEntry modInfo = null;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            AssetLoader.ModEntry = modEntry;
            modInfo = modEntry;
            harmony.PatchAll();
            return true;
        }
        public static void Log(string msg)
        {
            modInfo.Logger.Log(msg);
        }
    }
}
