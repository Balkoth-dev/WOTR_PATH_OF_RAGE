using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.Abilities;
using BlueprintCore;
using BlueprintCore.Blueprints;
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

namespace WOTR_PATH_OF_RAGE.Spells
{
    class BloodHaze
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
                PatchBloodHaze();
            }
            static void PatchBloodHaze()
            {
                var bloodHaze = BlueprintTool.Get<BlueprintAbility>("04b0ea02e1db66c44a8c31b0d0badff8");

                var bloodHazeDescription = "You make your blood boil, making you faster and more ferocious. For 1 {g|Encyclopedia:Combat_Round}round{/g} per two caster levels," +
                                           " you gain an extra attack that stacks with the effects of the Haste {g|Encyclopedia:Spell}spell{/g} and a 30ft enhancement bonus to movement speed.\nAdditionally, every time you reduce a creature below zero {g|Encyclopedia:HP}HP{/g}," +
                                           " you prolong the effect. If the killed creature had more HD than your {g|Encyclopedia:Caster_Level}caster level{/g}, the duration is increased by 3 rounds. " +
                                           "Otherwise, it is increased by 1 round.";

                bloodHaze.m_Description = Helpers.CreateString(bloodHaze + ".Description", bloodHazeDescription);

                bloodHaze.RemoveComponents<BuffExtraAttack>();
                bloodHaze.AddComponent<BuffExtraAttack>(c => c.Number = 1);

                Main.Log("Blood Haze Patched");

            }
        }
    }
}
