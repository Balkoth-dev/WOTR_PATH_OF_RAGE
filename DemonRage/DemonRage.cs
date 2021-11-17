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
                PatchRagesForDemon();
            }

            static void PatchDemonRage()
            {
                var abyssalStorm = BlueprintTool.Get<BlueprintAbility>("58e9e2883bca1574e9c932e72fd361f9");

                var demonicRageActivatableAbility = BlueprintTool.Get<BlueprintActivatableAbility>("0999f99d6157e5c4888f4cfe2d1ce9d6");
                    demonicRageActivatableAbility.OnlyInCombat = false;
                    demonicRageActivatableAbility.DeactivateImmediately = false;
                    demonicRageActivatableAbility.DeactivateIfCombatEnded = true;
            //        demonicRageActivatableAbility.ActivationType = new AbilityActivationType();
                    demonicRageActivatableAbility.EditComponent<ActivatableAbilityResourceLogic>(c => c.SpendType = ActivatableAbilityResourceLogic.ResourceSpendType.NewRound);
                    demonicRageActivatableAbility.m_Icon = abyssalStorm.m_Icon;

                var demonRageFeature = BlueprintTool.Get<BlueprintFeature>("6a8af3f208a0fa747a465b70b7043019");
                demonRageFeature.RemoveComponents<AddFacts>();
                demonRageFeature.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                    demonicRageActivatableAbility.ToReference<BlueprintUnitFactReference>()
                };
                }); 
                demonRageFeature.m_Icon = abyssalStorm.m_Icon;

                ///

                var demonSmash = BlueprintTool.Get<BlueprintAbility>("3013d6463fa34ff8bb46eba61b6ed581");

                demonRageFeature.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                    demonSmash.ToReference<BlueprintUnitFactReference>()
                };
                });

                Main.Log("Demonic Smash Added To Mythic");
                ///

                var demonRageBuff = BlueprintTool.Get<BlueprintBuff>("36ca5ecd8e755a34f8da6b42ad4c965f");
                demonRageBuff.m_Icon = abyssalStorm.m_Icon;

             //   demonRageBuff.RemoveComponents<CombatStateTrigger>();

                var demonRageResource = BlueprintTool.Get<BlueprintAbilityResource>("f3bf174f0f86b4f45a823e9ed6ccc7a5");

                demonRageResource.m_MaxAmount.BaseValue = 11;
                demonRageResource.m_MaxAmount.StartingLevel = 1;
                demonRageResource.m_MaxAmount.LevelStep = 1;
                demonRageResource.m_MaxAmount.PerStepIncrease = 6;
                demonRageResource.m_Max = 100;

                var standartRageActivateableAbility = BlueprintTool.Get<BlueprintActivatableAbility>("df6a2cce8e3a9bd4592fb1968b83f730");
                standartRageActivateableAbility.RemoveComponents<RestrictionHasFact>();

                var bloodragerStandartRageActivateableAbility = BlueprintTool.Get<BlueprintActivatableAbility>("e3a0056eedac7754ca9a50603ba05177");
                bloodragerStandartRageActivateableAbility.RemoveComponents<RestrictionHasFact>();

                Main.Log("Patching Demonic Rage Complete");
            }
        }

        static void PatchRagesForDemon()
        {
            var standartRageActivateableAbility = BlueprintTool.Get<BlueprintActivatableAbility>("df6a2cce8e3a9bd4592fb1968b83f730");
            standartRageActivateableAbility.RemoveComponents<RestrictionHasFact>();

            var bloodragerStandartRageActivateableAbility = BlueprintTool.Get<BlueprintActivatableAbility>("e3a0056eedac7754ca9a50603ba05177");
            bloodragerStandartRageActivateableAbility.RemoveComponents<RestrictionHasFact>();

            Main.Log("Patching Barbarian and Bloodrager Rages Complete");
        }
    }
}
