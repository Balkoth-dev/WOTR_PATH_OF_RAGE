using HarmonyLib;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;

namespace WOTR_PATH_OF_RAGE.MechanicsChanges
{
    class HasAnyFlagFix
    {
        [HarmonyPatch(typeof(SpellDescriptorOperations), "HasAnyFlag")]
        class Patch
        {
            static void Postfix(ref bool __result)
            {
                if (__result == false)
                {
                    __result = true;
                }
                Main.Log("PostFix");
            }
        }

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        class Patch2
        {
            static void Postfix()
            {
                Main.Log("BLUBBBLES");
            }
        }
    }
}
