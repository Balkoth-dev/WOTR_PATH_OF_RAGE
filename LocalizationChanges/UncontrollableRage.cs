using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Blueprints.Classes;
using WOTR_PATH_OF_RAGE.Utilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Enums;
using WOTR_PATH_OF_RAGE.MechanicsChanges;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.ElementsSystem;
using System;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic;
using Kingmaker.EntitySystem.Stats;

namespace WOTR_PATH_OF_RAGE.LocalizationChanges
{
    class DemonicAspects
    {
        static BlueprintFeature demonRageForcedRageFeature = BlueprintTool.Get<BlueprintFeature>("2a5d1de842d4c514495a195a808b14c9");
        static BlueprintBuff demonRageBuffUncontrollable = BlueprintTool.Get<BlueprintBuff>("325e00281f7e4a54cbc60627f2f66cec");
        static BlueprintBuff artifact_DemonCloakBuff = BlueprintTool.Get<BlueprintBuff>("4643e8c14921459693acf1cc8e7d998d");

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;

                Main.Log("Patching Localization Issues");
                PatchUncontrollableRageLocalization();
                PatchUncontrollableRageToggle();
                PatchUncontrollableRageBuff();
            }

            static void PatchUncontrollableRageLocalization()
            {
                if(!Main.settings.UncontrollableRageLocalization)
                {
                    return;
                }

                var nameString = "Uncontrolled Rage";
                var descriptionString = "When paralyzed, frightened, or stunned, you become immune to all movement-impairing effects. " +
                    "This immunity includes but is not limited to paralysis, petrification, confusion, sleep, stun, fear, and daze.\n" +
                    "Additionally, your weapon increases by two size categories, your base attack bonus is equal to your character level, " +
                    "and you gain a +6 enhancement bonus to strength, dexterity, and constitution. You also gain a +4 natural armor bonus to your AC.\n" +
                    "Furthermore, your base strength is set to a minimum of double your mythic level + 16. However, while under this effect, you cannot distinguish " +
                    "between an ally and an enemy, and you lose control of yourself, attacking the nearest creature.";

                demonRageForcedRageFeature.m_DisplayName = Helpers.CreateString(demonRageForcedRageFeature + ".Name", nameString);
                demonRageBuffUncontrollable.m_DisplayName = Helpers.CreateString(demonRageBuffUncontrollable + ".Name", nameString);

                demonRageForcedRageFeature.m_Description = Helpers.CreateString(demonRageForcedRageFeature + ".Description", descriptionString);
                demonRageBuffUncontrollable.m_Description = Helpers.CreateString(demonRageBuffUncontrollable + ".Description", descriptionString);

                Main.Log("Uncontrollable Rage Localization Updated");

            }

            static void PatchUncontrollableRageToggle()
            {
                var demonUncontrollableBuffGuid = new BlueprintGuid(new Guid("6085f92c-8222-4d6c-a57d-41f8c514c635"));

                var demonUncontrollableBuff = Helpers.Create<BlueprintBuff>(c =>
                {
                    c.AssetGuid = demonUncontrollableBuffGuid;
                    c.name = "Uncontrollable Rage Buff " + c.AssetGuid;
                    c.m_Icon = demonRageBuffUncontrollable.m_Icon;
                    c.m_DisplayName = demonRageBuffUncontrollable.m_DisplayName;
                    c.m_Description = demonRageBuffUncontrollable.m_Description;
                    c.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                });

                Helpers.AddBlueprint(demonUncontrollableBuff, demonUncontrollableBuffGuid);

                var demonUncontrollableAAGuid = new BlueprintGuid(new Guid("15153326-5f33-43d9-a21c-eda522aec033"));

                var demonUncontrollableAA = Helpers.Create<BlueprintActivatableAbility>(c =>
                {
                    c.AssetGuid = demonUncontrollableAAGuid;
                    c.name = "Uncontrollable Rage" + c.AssetGuid;
                    c.m_Icon = demonRageBuffUncontrollable.m_Icon;
                    c.m_Buff = demonUncontrollableBuff.ToReference<BlueprintBuffReference>();
                    c.ActivationType = new AbilityActivationType();
                    c.DeactivateIfCombatEnded = true;
                    c.m_DisplayName = demonRageBuffUncontrollable.m_DisplayName;
                    c.m_Description = demonRageBuffUncontrollable.m_Description;
                });


                Main.Log("Uncontrollable Rage toggle blueprints added");

                Helpers.AddBlueprint(demonUncontrollableAA, demonUncontrollableAAGuid);

                if (!Main.settings.UncontrollableRageToggle)
                {
                    return;
                }

                demonUncontrollableBuff.AddComponent<BuffSubstitutionOnApply>(c =>
                {
                    c.SpellDescriptor = SpellDescriptor.Frightened | SpellDescriptor.Stun | SpellDescriptor.Paralysis;
                    c.m_SubstituteBuff = demonRageBuffUncontrollable.ToReference<BlueprintBuffReference>();
                    c.CheckDescriptor = true;
                });

                demonRageForcedRageFeature.RemoveComponents<BuffSubstitutionOnApply>();

                demonRageForcedRageFeature.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[]{
                        demonUncontrollableAA.ToReference<BlueprintUnitFactReference>()
                    };
                });

                Main.Log("Uncontrollable Rage Updated");

            }
        }
        static void PatchUncontrollableRageBuff()
        {
            if (!Main.settings.UncontrollableRageFix)
            {
                return;
            }

            demonRageBuffUncontrollable.RemoveAllComponents();

            demonRageBuffUncontrollable.AddComponent<AddConditionImmunity>(c => {
                c.Condition = UnitCondition.Paralyzed | UnitCondition.Dazed | UnitCondition.Petrified | UnitCondition.Frightened | UnitCondition.Sleeping | UnitCondition.CantMove |
                              UnitCondition.Stunned | UnitCondition.Confusion | UnitCondition.CantAct | UnitCondition.CanNotAttack | UnitCondition.Unconscious | UnitCondition.MovementBan |
                              UnitCondition.Cowering | UnitCondition.Helpless;
            });

            demonRageBuffUncontrollable.AddComponent<WeaponSizeChange>(c => {
                c.SizeCategoryChange = 2;
                c.CheckWeaponCategory = false;
                c.Category = WeaponCategory.UnarmedStrike;
            });

            demonRageBuffUncontrollable.AddComponent<AddStatBonus>(c =>
            {
                c.Descriptor = ModifierDescriptor.Enhancement;
                c.Stat = StatType.Strength | StatType.Dexterity | StatType.Constitution;
                c.Value = 6;
                c.ScaleByBasicAttackBonus = false;
            });

            demonRageBuffUncontrollable.AddComponent<AddStatBonus>(c =>
            {
                c.Descriptor = ModifierDescriptor.NaturalArmor;
                c.Stat = StatType.AC;
                c.Value = 4;
                c.ScaleByBasicAttackBonus = false;
            });

            demonRageBuffUncontrollable.AddComponent<ContextRankConfig>(c =>
            {
                c.m_Type = AbilityRankType.StatBonus;
                c.m_BaseValueType = ContextRankBaseValueType.MythicLevelPlusBuffRank;
                c.m_Stat = StatType.Unknown;
                c.m_SpecificModifier = ModifierDescriptor.None;
                c.m_Buff = artifact_DemonCloakBuff.ToReference<BlueprintBuffReference>();
                c.m_BuffRankMultiplier = 3;
                c.m_Progression = ContextRankProgression.DoublePlusBonusValue;
                c.m_StartLevel = 0;
                c.m_StepLevel = 16;
                c.m_UseMin = false;
                c.m_UseMax = false;
                c.m_Min = 0;
                c.m_Max = 20;
                c.m_ExceptClasses = false;
            });

            demonRageBuffUncontrollable.AddComponent<ContextRankConfig>(c =>
            {
                c.m_Type = AbilityRankType.Default;
                c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                c.m_Stat = StatType.Unknown;
                c.m_SpecificModifier = ModifierDescriptor.None;
                c.m_BuffRankMultiplier = 1;
                c.m_Progression = ContextRankProgression.AsIs;
                c.m_StartLevel = 0;
                c.m_StepLevel = 0;
                c.m_UseMin = false;
                c.m_UseMax = false;
                c.m_Min = 0;
                c.m_Max = 20;
                c.m_ExceptClasses = false;
            });

            demonRageBuffUncontrollable.AddComponent<RaiseStatToMinimum>(c =>
            {
                c.Stat = StatType.Strength;
                c.TargetValue = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    Value = 0,
                    ValueRank = AbilityRankType.StatBonus,
                    ValueShared = Kingmaker.UnitLogic.Abilities.AbilitySharedValue.Damage,
                    Property = UnitProperty.None,
                    m_AbilityParameter = AbilityParameterType.Level
                };
            });

            demonRageBuffUncontrollable.AddComponent<RaiseBAB>(c =>
            {
                c.TargetValue = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    Value = 0,
                    ValueRank = AbilityRankType.Default,
                    ValueShared = Kingmaker.UnitLogic.Abilities.AbilitySharedValue.Damage,
                    Property = UnitProperty.None,
                    m_AbilityParameter = AbilityParameterType.Level
                };
            });

            demonRageBuffUncontrollable.AddComponent<CombatStateTrigger>(c => {
                c.CombatStartActions = new ActionList();
                c.CombatEndActions = new ActionList() { Actions = new GameAction[] { Helpers.Create<ContextActionRemoveSelf>() } };
            });

            demonRageBuffUncontrollable.AddComponent<AddCondition>(c =>
            {
                c.Condition = UnitCondition.AttackNearest;
            });

            demonRageBuffUncontrollable.AddComponent<AddContextStatBonus>(c =>
            {
                c.Descriptor = ModifierDescriptor.None;
                c.Stat = StatType.Speed;
                c.Multiplier = 1;
                c.HasMinimal = false;
                c.Minimal = 0;
                c.Value = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    Value = 0,
                    ValueRank = AbilityRankType.StatBonus,
                    ValueShared = Kingmaker.UnitLogic.Abilities.AbilitySharedValue.Damage,
                    Property = UnitProperty.None,
                    m_AbilityParameter = AbilityParameterType.Level
                };
            });


            Main.Log("Uncontrollable Rage Fix Updated");

        }
    }
}

