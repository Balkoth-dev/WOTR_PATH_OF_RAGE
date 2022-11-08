using HarmonyLib;
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
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using WOTR_PATH_OF_RAGE.NewRules;

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
                var demonRageAbility = BlueprintTool.Get<BlueprintActivatableAbility>("0999f99d6157e5c4888f4cfe2d1ce9d6");
                Main.Log(demonRageAbility.DeactivateIfCombatEnded.ToString());
            }

            static void PatchDemonRage()
            {
                var outOfCombatBuff = OutOfCombatBuff();

                if (Main.settings.PatchDemonRage == false)
                {
                    return;
                }

                BlueprintAbilityResourceReference newDemonRageResource = BlueprintTool.Get<BlueprintAbilityResource>("bc2c2f64ada54c78a250f8b72c48ae57").ToReference<BlueprintAbilityResourceReference>();

                var demonRageAbility = BlueprintTool.Get<BlueprintActivatableAbility>("0999f99d6157e5c4888f4cfe2d1ce9d6");
                    demonRageAbility.m_ActivateOnUnitAction = AbilityActivateOnUnitActionType.Attack;
                    demonRageAbility.ActivationType = AbilityActivationType.Immediately;
                    demonRageAbility.DeactivateIfCombatEnded = false;
                    demonRageAbility.OnlyInCombat = false;
                    demonRageAbility.DeactivateImmediately = true;

                demonRageAbility.EditComponent<ActivatableAbilityResourceLogic>(c => { c.SpendType = ActivatableAbilityResourceLogic.ResourceSpendType.NewRound; c.m_RequiredResource = newDemonRageResource; c.m_FreeBlueprint = outOfCombatBuff.ToReference<BlueprintUnitFactReference>(); }); ;
                demonRageAbility.AddComponent<RestrictionUnitHasResource>(c => { c.m_resource = newDemonRageResource; });

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
                    demonRageAbility.ToReference<BlueprintUnitFactReference>(),
                    outOfCombatBuff.ToReference<BlueprintUnitFactReference>()
                };
                });

                var applyOutOfCombatBuff = new ContextActionApplyBuff()
                {
                    m_Buff = outOfCombatBuff.ToReference<BlueprintBuffReference>(),
                    Permanent = true,
                    DurationValue = new ContextDurationValue(),
                    AsChild = true,
                    IsFromSpell = false,
                    IsNotDispelable = false,
                    ToCaster = false,
                    SameDuration = false,
                    UseDurationSeconds = false,
                    DurationSeconds = 0
                };

                var removeOutOfCombatBuff = new ContextActionRemoveBuff()
                {
                    m_Buff = outOfCombatBuff.ToReference<BlueprintBuffReference>(),
                    OnlyFromCaster = false,
                    ToCaster = true
                };

                var conditionalBuffEffects = new Conditional()
                {
                    ConditionsChecker = new ConditionsChecker()
                    {
                        Operation = Operation.And,
                        Conditions = new Condition[] {
                            new ContextConditionIsInCombat(){Not = true }
                        }
                    },
                    IfFalse = new ActionList() { Actions = new GameAction[] { removeOutOfCombatBuff } },
                    IfTrue = new ActionList() { Actions = new GameAction[] { applyOutOfCombatBuff } }
                };

                demonRageFeature.AddComponent<NewRoundTrigger>(c => {
                    c.NewRoundActions = new ActionList();
                    c.NewRoundActions.Actions = new GameAction[] { conditionalBuffEffects };
                });

                demonRageFeature.AddComponent<CombatStateTrigger>(c => {
                    c.CombatStartActions = new ActionList();
                    c.CombatStartActions.Actions = new GameAction[] { Helpers.Create<ContextActionRemoveBuff>(c => { c.m_Buff = outOfCombatBuff.ToReference<BlueprintBuffReference>(); c.ToCaster = true; }) };
                    c.CombatEndActions = new ActionList();
                    c.CombatEndActions.Actions = new GameAction[] { applyOutOfCombatBuff };
                });

                demonRageFeature.RemoveComponents<AddAbilityResources>();
                demonRageFeature.AddComponent<AddAbilityResources>(c =>
                {
                    c.RestoreAmount = true;
                    c.m_Resource = newDemonRageResource;
                });

                demonRageFeature.m_Icon = demonRageAbility.m_Icon;

                var demonRageBuff = BlueprintTool.Get<BlueprintBuff>("36ca5ecd8e755a34f8da6b42ad4c965f");
                demonRageBuff.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonRage.png");

                demonRageBuff.RemoveComponents<CombatStateTrigger>();

                var removeDemonRageBuff = new ContextActionRemoveSelf();

                conditionalBuffEffects = new Conditional()
                {
                    ConditionsChecker = new ConditionsChecker()
                    {
                        Operation = Operation.And,
                        Conditions = new Condition[] {
                            new ContextConditionIsInCombat(){Not = true },
                            new ContextConditionHasFact(){Not = true, m_Fact = demonRageFeature.ToReference<BlueprintUnitFactReference>()}
                        }
                    },
                    IfFalse = new ActionList() { Actions = new GameAction[] { } },
                    IfTrue = new ActionList() { Actions = new GameAction[] { removeDemonRageBuff } }
                };

                demonRageBuff.AddComponent<NewRoundTrigger>(c => {
                    c.NewRoundActions = new ActionList();
                    c.NewRoundActions.Actions = new GameAction[] { conditionalBuffEffects };
                });

                AddElementalRampage(demonRageBuff);

                Main.Log("Patching Demonic Rage Complete");
            }

            private static void AddElementalRampage(BlueprintBuff demonRageBuff)
            {
                var elementalRampagerRampageFeature = BlueprintTool.Get<BlueprintFeature>("64c5dfe0ba664dd38b7e914ef0912a1c").ToReference<BlueprintUnitFactReference>();
                var elementalRampagerRampageBuff = BlueprintTool.Get<BlueprintBuff>("98798aa2d21a4c20a31d31527642b5f5");
                var elementalRampagerRampageBuffRef = BlueprintTool.Get<BlueprintBuff>("98798aa2d21a4c20a31d31527642b5f5").ToReference<BlueprintBuffReference>(); ;

                elementalRampagerRampageBuff.AddComponent<ContextCalculateSharedValue>(c => { c.ValueType = Kingmaker.UnitLogic.Abilities.AbilitySharedValue.Duration; c.Value = new ContextDiceValue() { DiceType = Kingmaker.RuleSystem.DiceType.Zero, BonusValue = new ContextValue(), DiceCountValue = new ContextValue() }; c.Modifier = 1; });

                var hasElementalRampageCondition = new Condition[] {
                            new ContextConditionHasFact
                            {
                                m_Fact = elementalRampagerRampageFeature,
                                Not = false
                            }
                };

                var hasElementalRampageConditionsChecker = new ConditionsChecker()
                {
                    Conditions = hasElementalRampageCondition
                };

                var applyElementalRampageBuff = new ContextActionApplyBuff()
                {
                    m_Buff = elementalRampagerRampageBuffRef,
                    Permanent = true,
                    DurationValue = new ContextDurationValue(),
                    AsChild = true,
                    IsFromSpell = false,
                    IsNotDispelable = true,
                    ToCaster = false,
                    SameDuration = false,
                    UseDurationSeconds = false,
                    DurationSeconds = 0
                };

                var conditionalElementalRampage = new Conditional()
                {
                    ConditionsChecker = hasElementalRampageConditionsChecker,
                    IfTrue = new ActionList()
                    {
                        Actions = new GameAction[] { applyElementalRampageBuff }
                    },
                    IfFalse = new ActionList()
                };

                demonRageBuff.EditComponent<AddFactContextActions>(c => { c.Activated.Actions = c.Activated.Actions.AppendToArray(conditionalElementalRampage); });
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

        static BlueprintBuff OutOfCombatBuff()
        {
            var outOfCombatBuffGuid = new BlueprintGuid(new Guid("92b4d9eb-d91d-4759-b9a9-0423c9a587b5"));

            var outOfCombatBuff = Helpers.Create<BlueprintBuff>(c =>
            {
                c.AssetGuid = outOfCombatBuffGuid;
                c.name = "Out Of Combat" + c.AssetGuid;
                c.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                c.Components = new BlueprintComponent[] { };
                c.Stacking = StackingType.Replace;
            });

            Helpers.AddBlueprint(outOfCombatBuff, outOfCombatBuffGuid);

            return outOfCombatBuff;
        }
    }
}
