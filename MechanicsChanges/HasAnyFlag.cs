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
            static void Postfix(SpellDescriptor descriptor, SpellDescriptor flags, ref bool __result)
            {
                __result |= flags == SpellDescriptor.None;
            }
        }
    }
}
