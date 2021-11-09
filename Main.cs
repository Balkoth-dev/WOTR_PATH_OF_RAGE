using HarmonyLib;
using UnityModManagerNet;

namespace WOTR_PATH_OF_RAGE
{
    public class Main
    {
        public static UnityModManager.ModEntry modInfo = null;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
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
