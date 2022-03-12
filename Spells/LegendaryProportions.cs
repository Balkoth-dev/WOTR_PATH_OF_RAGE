using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using BlueprintCore.Utils;
using BlueprintCore;

using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Blueprints;
using WOTR_PATH_OF_RAGE.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using WOTR_PATH_OF_RAGE.Utilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Classes.Spells;
using WOTR_PATH_OF_RAGE.MechanicsChanges;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.ElementsSystem;

namespace WOTR_PATH_OF_RAGE.Spells
{
    class LegendaryProportions
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;

                Main.Log("Patching Blood Haze");
                PatchLegendaryProportions();
            }
            static void PatchLegendaryProportions()
            {
                if (Main.settings.PatchLegendaryProportions == false)
                {
                    return;
                }
                var legendaryProportionsBuff = BlueprintTool.Get<BlueprintBuff>("4ce640f9800d444418779a214598d0a3");

                legendaryProportionsBuff.EditComponent<ChangeUnitSize>(c => c.SizeDelta = 1);

                Main.Log("Legendary Proportions Patched");

            }
        }
    }
}
