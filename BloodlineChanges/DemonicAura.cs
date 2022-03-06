using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using WOTR_PATH_OF_RAGE.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.RuleSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using WOTR_PATH_OF_RAGE.MechanicsChanges;
using Kingmaker.Designers.Mechanics.Facts;

namespace WOTR_PATH_OF_RAGE.BloodlineChanges
{
    class DemonicAura
    {
        public static void DemonicAuraPatch()
        {
            var bloodragerAbyssalDemonicAura = BlueprintTool.Get<BlueprintFeature>("677171968b9a22f459c7391d8302dd36");
            var demonSmashProjectile = BlueprintTool.Get<BlueprintProjectile>("fec53329817f4aa6a9210be9867a8930");

            var demonAuraBuffGuid = new BlueprintGuid(new Guid("aa8f461e04104b8099437b9806f6ee17"));

            var demonAuraBuff = Helpers.Create<BlueprintBuff>(c =>
            {
                c.AssetGuid = demonAuraBuffGuid;
                c.name = "demonAuraBuff" + c.AssetGuid;
                c.m_Icon = bloodragerAbyssalDemonicAura.m_Icon;
                c.m_Description = bloodragerAbyssalDemonicAura.m_Description;
                c.m_DisplayName = bloodragerAbyssalDemonicAura.m_DisplayName;
                c.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            });

            demonAuraBuff.AddComponent<ContextRankConfig>(c =>
            {
                c.m_Type = AbilityRankType.DamageBonus;
                c.m_BaseValueType = ContextRankBaseValueType.StatBonus;
                c.m_Stat = Kingmaker.EntitySystem.Stats.StatType.Constitution;
                c.m_StepLevel = 0;
            });

            var demonAuraFireDamage = Helpers.Create<ContextActionDealDamage>(c =>
            {
                c.DamageType = new DamageTypeDescription
                {
                    Type = DamageType.Energy,
                    Energy = DamageEnergyType.Fire
                };
                c.Duration = new ContextDurationValue()
                {
                    m_IsExtendable = true,
                    DiceCountValue = new ContextValue(),
                    BonusValue = new ContextValue()
                };
                c.Value = new ContextDiceValue
                {
                    DiceType = DiceType.D6,
                    DiceCountValue = new ContextValue()
                    {
                        Value = 2
                    },
                    BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageBonus
                    }
                };
                c.m_IsAOE = true;

            });

            demonAuraBuff.AddComponent<NewRoundTrigger>(c =>
            {
                c.NewRoundActions = new ActionList();
                c.NewRoundActions.Actions = new GameAction[] { demonAuraFireDamage };
            });

            Helpers.AddBlueprint(demonAuraBuff, demonAuraBuffGuid);

            if (Main.settings.PatchDemonicAura == false)
            {
                return;
            };
            var bloodragerAbyssalDemonicAuraEffectBuff = BlueprintTool.Get<BlueprintBuff>("2fb5abfcd97cede4a8efad96aa3cf9d3");
            var bloodragerAbyssalDemonicAuraAura = BlueprintTool.Get<BlueprintAbilityAreaEffect>("408578c9ca390bf4dadd84c962c558df");


            var demonRipIsNotSelf = new ConditionsChecker()
            {
                Conditions = new Condition[] {
                            new ContextConditionIsCaster(){Not = true}
                }
            };
            bloodragerAbyssalDemonicAuraAura.RemoveComponents<AbilityAreaEffectRunAction>();
           
            bloodragerAbyssalDemonicAuraAura.AddComponent<AbilityAreaEffectBuff>(c =>
            {
                c.m_Buff = demonAuraBuff.ToReference<BlueprintBuffReference>();
                c.Condition = demonRipIsNotSelf;
            });

            bloodragerAbyssalDemonicAura.m_Description = Helpers.CreateString(bloodragerAbyssalDemonicAura + ".Description", "At 16th level, when entering a bloodrage you can choose " +
                                                                                                                             "to exude an aura of fire. The aura is a 5-foot burst centered on you, " +
                                                                                                                             "and deals ({g|Encyclopedia:Dice}2d6{/g} + your {g|Encyclopedia:Constitution}Constitution{/g} " +
                                                                                                                             "modifier) points of {g|Encyclopedia:Energy_Damage}fire damage{/g} to creatures that start their " +
                                                                                                                             "turns within it.");

        }

    }
}
