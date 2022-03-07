using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
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
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintCore.Utils;

namespace WOTR_PATH_OF_RAGE.DemonProgression
{
    class DemonProgression
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;
            static BlueprintProgression demonProgression;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;
                demonProgression = BlueprintTool.Get<BlueprintProgression>("285fe49f7df8587468f676aa49362213");
                Main.Log("Patching Demon Progression");
                AddDemonSpecialSelectionToMythic();
                PatchDemonLordAspects();
            }

            public static void AddDemonSpecialSelectionToMythic()
            {
                if (Main.settings.AddDemonSpecialSelectionToMythic == false)
                {
                    return;
                }
                var demonSpecialSelection = BlueprintTool.Get<BlueprintFeatureSelection>("1df9edd3e5f4485793e57a40e1d567f2");

                demonProgression.LevelEntries[0].m_Features.Add(demonSpecialSelection.ToReference<BlueprintFeatureBaseReference>());

                var demonHungerPolymorphFeature = BlueprintTool.Get<BlueprintFeature>("6c8f47ae288f44d5bfa621d2c91b7594");

                demonProgression.LevelEntries[6].m_Features.Add(demonHungerPolymorphFeature.ToReference<BlueprintFeatureBaseReference>());

                Main.Log("Demonic Specials Added To Mythic");
            }
            public static void PatchDemonLordAspects()
            {
                return; // This was fixed in 1.2, ignoring settings.
                if (Main.settings.PatchDemonLordAspects == false)
                {
                    return;
                }
                var demonLordAspectFeature = BlueprintTool.Get<BlueprintFeatureSelection>("fc93daa527ec58c40afbe874c157bc91").ToReference<BlueprintFeatureBaseReference>();

                demonProgression.LevelEntries[6].m_Features.Add(demonLordAspectFeature);
                demonProgression.LevelEntries[7].m_Features.Remove(demonLordAspectFeature);



                Main.Log("Demonic Lord Aspect Progression Patched");
            }
        }
    }
}