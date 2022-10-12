﻿using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
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
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using System;

namespace WOTR_PATH_OF_RAGE.DemonRage
{
    class DemonRage
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;

                Main.Log("Patching Demonic Rage");
                PatchDemonRage();
                PatchBaseRagesForDemon();
            }

            static void PatchDemonRage()
            {

                var demonRageResource = BlueprintTool.Get<BlueprintAbilityResource>("f3bf174f0f86b4f45a823e9ed6ccc7a5");

                var newDemonRageResourceGuid = new BlueprintGuid(new Guid("bc2c2f64-ada5-4c78-a250-f8b72c48ae57"));

                var newDemonRageResource = Helpers.CreateCopy(demonRageResource, bp =>
                {
                    bp.AssetGuid = newDemonRageResourceGuid;
                    bp.name = "New Demon Rage Resource" + bp.AssetGuid;
                });

                newDemonRageResource.m_MaxAmount.BaseValue = 11;
                newDemonRageResource.m_MaxAmount.StartingLevel = 1;
                newDemonRageResource.m_MaxAmount.LevelStep = 1;
                newDemonRageResource.m_MaxAmount.PerStepIncrease = 3;

                Helpers.AddBlueprint(newDemonRageResource, newDemonRageResourceGuid);

                if (Main.settings.PatchDemonRage == false)
                {
                    return;
                }

                var demonRageAbility = BlueprintTool.Get<BlueprintActivatableAbility>("0999f99d6157e5c4888f4cfe2d1ce9d6");
                    demonRageAbility.OnlyInCombat = false;
                    demonRageAbility.DeactivateImmediately = true;
                    demonRageAbility.DeactivateIfCombatEnded = true;
                    demonRageAbility.IsOnByDefault = true;
                    demonRageAbility.m_ActivateOnUnitAction = AbilityActivateOnUnitActionType.Attack;
    
                demonRageAbility.ActivationType = new AbilityActivationType();
                demonRageAbility.EditComponent<ActivatableAbilityResourceLogic>(c => { c.SpendType = ActivatableAbilityResourceLogic.ResourceSpendType.NewRound; c.m_RequiredResource = newDemonRageResource.ToReference<BlueprintAbilityResourceReference>(); }); ;
                    demonRageAbility.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonRage.png");
                var demonRageDescription = "The power of the Abyss courses through the Demon waiting to be unleashed.\n" +
                    "The Demon can enter a demonic rage as a {g|Encyclopedia:Free_Action}free action{/g}.\n" +
                    "While in demonic rage, the Demon gains +2 {g|Encyclopedia:Bonus}bonus{/g} on {g|Encyclopedia:Attack}attack rolls{/g}, {g|Encyclopedia:Damage}damage rolls{/g}, " +
                    "{g|Encyclopedia:Caster_Level}caster level{/g} {g|Encyclopedia:Check}checks{/g} and Reflex saving throws.\nThe {g|Encyclopedia:DC}DC{/g} for all " +
                    "{g|Encyclopedia:Saving_Throw}saving throws{/g} against Demon's {g|Encyclopedia:Spell}spells{/g} and abilities are increased by 2.\n" +
                    "These benefits increase by 1 at 6th and 9th mythic ranks.\n" +
                    "You may enact your rage this way for 12 rounds with an additional 3 rounds per mythic rank.";
                demonRageAbility.m_Description = Helpers.CreateString(demonRageAbility + ".Description", demonRageDescription);

                var demonRageFeature = BlueprintTool.Get<BlueprintFeature>("6a8af3f208a0fa747a465b70b7043019");
                demonRageFeature.m_Description = demonRageAbility.m_Description;
                demonRageFeature.RemoveComponents<AddFacts>();
                demonRageFeature.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                    demonRageAbility.ToReference<BlueprintUnitFactReference>()
                };
                });

                demonRageFeature.RemoveComponents<AddAbilityResources>();
                demonRageFeature.AddComponent<AddAbilityResources>(c =>
                {
                    c.RestoreAmount = true;
                    c.m_Resource = newDemonRageResource.ToReference<BlueprintAbilityResourceReference>();
                });

                demonRageFeature.m_Icon = demonRageAbility.m_Icon;

                var demonRageBuff = BlueprintTool.Get<BlueprintBuff>("36ca5ecd8e755a34f8da6b42ad4c965f");
                demonRageBuff.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonRage.png");
                
                Main.Log("Patching Demonic Rage Complete");
            }
        }

        static void PatchBaseRagesForDemon()
        {
            var standartRageActivateableAbility = BlueprintTool.Get<BlueprintActivatableAbility>("df6a2cce8e3a9bd4592fb1968b83f730");
            standartRageActivateableAbility.RemoveComponents<RestrictionHasFact>();
            standartRageActivateableAbility.DeactivateImmediately = true;

            var bloodragerStandartRageActivateableAbility = BlueprintTool.Get<BlueprintActivatableAbility>("e3a0056eedac7754ca9a50603ba05177");
            bloodragerStandartRageActivateableAbility.RemoveComponents<RestrictionHasFact>();
            bloodragerStandartRageActivateableAbility.DeactivateImmediately = true;

            Main.Log("Patching Barbarian and Bloodrager Rages Complete");
        }
    }
}
