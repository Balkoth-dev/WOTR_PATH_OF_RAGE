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
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.ElementsSystem;

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
                if (Main.settings.PatchBloodHaze == false)
                {
                    return;
                }
                var bloodHaze = BlueprintTool.Get<BlueprintAbility>("68f4129f658d02244ad57a4bfe3a5e61");
                var bloodHazeBuff = BlueprintTool.Get<BlueprintBuff>("173af01d6aae5574ba0391e277e9b168");

                var bloodHazeDescription = "You make your blood boil, making you faster and more ferocious. For 1 {g|Encyclopedia:Combat_Round}round{/g} per two caster levels, " +
                    "you gain the effects of the Haste {g|Encyclopedia:Spell}spell{/g} and a +2 Profane bonus to attack.\n Additionally, every time you reduce a creature below zero {g|Encyclopedia:HP}HP{/g}, you " +
                    "prolong the effect. If the killed creature had more HD than your {g|Encyclopedia:Caster_Level}caster level{/g}, the duration is increased by 3 rounds. " +
                    "Otherwise, it is increased by 1 round.";

                bloodHaze.m_Description = Helpers.CreateString(bloodHaze + ".Description", bloodHazeDescription);
                bloodHazeBuff.m_Description = bloodHaze.m_Description;

                bloodHazeBuff.AddComponent<AddContextStatBonus>(c =>
                {
                    c.Multiplier = 2;
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus;
                    c.Descriptor = ModifierDescriptor.Profane;
                    c.Value = 1;
                });

                Main.Log("Blood Haze Patched");

            }
        }
    }
}
