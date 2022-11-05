
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using WOTR_PATH_OF_RAGE.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using WOTR_PATH_OF_RAGE;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Localization;
using UnityEngine;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.RuleSystem;
using Kingmaker.Enums;
using Kingmaker.ElementsSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using WOTR_PATH_OF_RAGE.New_Rules;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using WOTR_PATH_OF_RAGE.MechanicsChanges;
using Kingmaker.Blueprints.Items.Ecnchantments;
using static Kingmaker.UnitLogic.UnitState;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.Designers.Mechanics.Buffs;
using BlueprintCore.Utils;

namespace WOTR_PATH_OF_RAGE.NewFeatures
{
    class DemonRip
    {
        public static void AddDemonRip()
        {
            var demonRageResource = BlueprintTool.Get<BlueprintAbilityResource>("bc2c2f64ada54c78a250f8b72c48ae57").ToReference<BlueprintAbilityResourceReference>();
            var destructionDomainGreaterAura = BlueprintTool.Get<BlueprintAbilityAreaEffect>("5a6c8bb6faf11fc4bb1022c3683d12d3");
            var demonBlastAbility = BlueprintTool.Get<BlueprintAbility>("6fc3b519185341449e4a4fd803b34d35");
            var fireball00 = BlueprintTool.Get<BlueprintProjectile>("8927afa172e0fc54484a29fa0c4c40c4");
            var demonSmashProjectile = BlueprintTool.Get<BlueprintProjectile>("fec53329817f4aa6a9210be9867a8930");

            var demonRipResource = BlueprintTool.Get<BlueprintAbilityResource>("d7a3af1bdffd4d31998271bffd04822f");

            var demonChargeProjectile = BlueprintTool.Get<BlueprintAbility>("4b18d0f44f57cbf4c91f094addfed9f4");
            ///

            var eldritchFontEldritchSurgeDCBuff = BlueprintTool.Get<BlueprintBuff>("91b2762997f0d8044baeeef0871eac6f");

            var demonRipBuffUnholyDamage = Helpers.Create<ContextActionDealDamage>(c =>
            {
                c.DamageType = new DamageTypeDescription
                {
                    Type = DamageType.Energy,
                    Energy = DamageEnergyType.Unholy
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
                        ValueType = ContextValueType.Rank,
                        Value = 0,
                        ValueRank = AbilityRankType.Default,
                        Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.None
                    },
                    BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageBonus
                    }
                };
                c.m_IsAOE = true;
            });

            var demonRipDebuffGuid = new BlueprintGuid(new Guid("b7805ba3-039e-4be9-ac34-17a54d85365f"));

            var demonRipDebuff = Helpers.CreateCopy(eldritchFontEldritchSurgeDCBuff, bp =>
            {
                bp.AssetGuid = demonRipDebuffGuid;
                bp.name = "carnage debuff" + bp.AssetGuid;
                bp.m_DisplayName = Helpers.CreateString(bp + ".Name", "Victim Of Carnage");
                bp.m_Description = Helpers.CreateString(bp + ".Description", "You have been hit in combat, when your turn starts you will take 1d6 damage per stack.");
                bp.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonRip.png");
                bp.Components = new BlueprintComponent[] { };
                bp.Stacking = StackingType.Rank;
                bp.Ranks = 10;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            });

            Helpers.AddBlueprint(demonRipDebuff, demonRipDebuffGuid);

            var removeDemonRipDebuff = new ContextActionRemoveBuff()
            {
                m_Buff = demonRipDebuff.ToReference<BlueprintBuffReference>(),
                OnlyFromCaster = false,
                ToCaster = true
            };

            demonRipDebuff.AddComponent<AddFactContextActions>(c =>
            {
                c.Activated = new ActionList();
                c.Activated.Actions = new GameAction[] { demonRipBuffUnholyDamage };
                c.Deactivated = new ActionList();
                c.NewRound = new ActionList();
                c.NewRound.Actions = new GameAction[] { demonRipBuffUnholyDamage, removeDemonRipDebuff };
            });

            demonRipDebuff.AddComponent<ContextRankConfig>(c =>
            {
                c.m_Type = AbilityRankType.Default;
                c.m_BaseValueType = ContextRankBaseValueType.TargetBuffRank;
                c.m_FeatureList = new BlueprintFeatureReference[] { };
                c.m_Stat = Kingmaker.EntitySystem.Stats.StatType.Unknown;
                c.m_SpecificModifier = ModifierDescriptor.None;
                c.m_Buff = demonRipDebuff.ToReference<BlueprintBuffReference>();
                c.m_Progression = ContextRankProgression.AsIs;
                c.m_StartLevel = 0;
                c.m_StepLevel = 0;
                c.m_UseMin = false;
                c.m_UseMax = false;
                c.m_Max = 20;
                c.m_ExceptClasses = false;
            });

            /////

            var demonRipBlastGuid = new BlueprintGuid(new Guid("f51dce1d-186d-4ca2-bf82-a0042d01e0e4"));

            var demonRipBlast = Helpers.Create<BlueprintAbility>(c =>
            {
                c.AssetGuid = demonRipBlastGuid;
                c.CanTargetPoint = true;
                c.CanTargetEnemies = true;
                c.CanTargetFriends = false;
                c.name = "Carnage Blast" + c.AssetGuid;
                c.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonRip.png");
                c.Range = AbilityRange.Unlimited;
                c.m_DisplayName = Helpers.CreateString(c + ".Name", "Carnage Blast");
                c.LocalizedSavingThrow = new LocalizedString();
                c.LocalizedDuration = new LocalizedString();
                c.m_Description = Helpers.CreateString(c + ".Description", "Deals 2d6 plus Mythic Rank damage in a 10 foot area.");
            });

            demonRipBlast.AddComponent<AbilityTargetsAround>(c =>
            {
                c.m_IncludeDead = true;
                c.m_Radius = new Kingmaker.Utility.Feet() { m_Value = 10 };
                c.m_TargetType = TargetType.Enemy;
                c.name = "AreaDemonSoulEffect";
                c.m_Condition = new ConditionsChecker()
                {
                    Conditions = new Condition[] { }
                };
            });

            var demonRipBlastFx = Helpers.Create<ContextActionProjectileFx>(c =>
            {
                c.m_Projectile = demonSmashProjectile.ToReference<BlueprintProjectileReference>();
            });

            var conditionalEffects = new Conditional()
            {
                ConditionsChecker = new ConditionsChecker()
                {
                    Conditions = new Condition[] {
                            new ContextConditionIsCaster() {
                                Not = true
                            }
                        }
                },
                IfTrue = new ActionList()
                {
                    Actions = new GameAction[] { demonRipBlastFx }
                },
                IfFalse = new ActionList()
            };

            demonRipBlast.AddComponent<ContextRankConfig>(c =>
            {
                c.m_Type = AbilityRankType.DamageBonus;
                c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                c.m_StepLevel = 2;
                c.m_Progression = ContextRankProgression.AsIs;
            });

            var demonRipUnholyDamage = Helpers.Create<ContextActionDealDamage>(c =>
            {
                c.DamageType = new DamageTypeDescription
                {
                    Type = DamageType.Energy,
                    Energy = DamageEnergyType.Unholy
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

            demonRipBlast.AddComponent<AbilityEffectRunAction>(c =>
            {
                c.Actions = new ActionList();
                c.Actions.Actions = new GameAction[] { conditionalEffects, demonRipUnholyDamage };
            });

            Helpers.AddBlueprint(demonRipBlast, demonRipBlastGuid);

            ///
            var demonRipBuffGuid = new BlueprintGuid(new Guid("d10450d4-9b06-4941-8c64-c182221c22e2"));

            var demonRipBuff = Helpers.Create<BlueprintBuff>(c =>
            {
                c.AssetGuid = demonRipBuffGuid;
                c.name = "Victim of Carnage" + c.AssetGuid;
                c.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonRip.png");
                c.m_DisplayName = Helpers.CreateString(c + ".Name", "Victim of Carnage");
                c.m_Description = Helpers.CreateString(c + ".Description", "\nThe target " +
                                                            "takes 1d6 Unholy damage per round. When attacked weapon attack they take an additional 1d6 unholy damage.\nWhen they die they deal 2d6 plus Mythic Rank of damage to enemies in a 10 foot radius");
            });

            var dismemberContext = Helpers.Create<ContextActionMarkForceDismemberOwner>(c =>
            {
                c.ForceDismemberType = DismemberType.InPower;
            });

            demonRipBuff.AddComponent<ActionsOnBuffApply>(c =>
            {
                c.Actions = new ActionList();
                c.Actions.Actions = new GameAction[] { dismemberContext };
            });

            var contextResourceIncrease = Helpers.Create<ContextActionIncreaseRageRounds>(c =>
            {
                c.m_resource = demonRageResource;
                c.m_resourceAmount = 1;
            });


            var demonRipUnholyDamage2 = Helpers.Create<ContextActionDealDamage>(c =>
            {
                c.DamageType = new DamageTypeDescription
                {
                    Type = DamageType.Energy,
                    Energy = DamageEnergyType.Unholy
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
                        Value = 1
                    },
                    BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        ValueRank = AbilityRankType.Default
                    }
                };
                c.m_IsAOE = true;
            });

            demonRipBuff.AddComponent<AddFactContextActions>(c =>
            {
                c.Activated = new ActionList();
                c.Activated.Actions = new GameAction[] { demonRipUnholyDamage2 };
                c.Deactivated = new ActionList();
                c.NewRound = new ActionList();
                c.NewRound.Actions = new GameAction[] { demonRipUnholyDamage2 };
            });

            var demonRipBuffContextActionCastSpell = Helpers.GenericAction<ContextActionCastSpellSimple>(c =>
            {
                c.m_Spell = demonRipBlast.ToReference<BlueprintAbilityReference>();
                c.DC = new ContextValue();
                c.SpellLevel = new ContextValue();
            });

            demonRipBuff.AddComponent<DeathActions>(c =>
            {
                c.Actions = new ActionList();
                c.Actions.Actions = new GameAction[] { contextResourceIncrease, demonRipBuffContextActionCastSpell };
            });

            var addDemonRipDebuff = new ContextActionApplyBuff()
            {
                m_Buff = demonRipDebuff.ToReference<BlueprintBuffReference>(),
                Permanent = true,
                UseDurationSeconds = false,
                DurationValue  = new ContextDurationValue(),
                DurationSeconds = 0,
                IsFromSpell = false,
                IsNotDispelable = false,
                ToCaster = false,
                AsChild = true,
                SameDuration = false
            };

            demonRipBuff.AddComponent<AddTargetAttackWithWeaponTrigger>(c => {
                c.ActionOnSelf = new ActionList();
                c.ActionOnSelf.Actions = new GameAction[] { addDemonRipDebuff };
                c.ActionsOnAttacker = new ActionList();
                c.OnlyHit = false;
            });

            Helpers.AddBlueprint(demonRipBuff, demonRipBuffGuid);
            ///

            var demonRipAreaGuid = new BlueprintGuid(new Guid("5d2b9742-ce82-457a-9ae7-209dce770071"));

            var demonAreaRip = Helpers.Create<BlueprintAbilityAreaEffect>(c =>
            {
                c.AssetGuid = demonRipAreaGuid;
                c.AggroEnemies = false;
                c.Shape = AreaEffectShape.Cylinder;
                c.Size = new Kingmaker.Utility.Feet() { m_Value = 30 };
                c.Fx = destructionDomainGreaterAura.Fx;
            });

            var demonRipIsEnemy = new ConditionsChecker()
            {
                Conditions = new Condition[] {
                            new ContextConditionIsEnemy(){}
                }
            };

            demonAreaRip.AddComponent<AbilityAreaEffectBuff>(c =>
            {
                c.m_Buff = demonRipBuff.ToReference<BlueprintBuffReference>();
                c.Condition = demonRipIsEnemy;
            });

            Helpers.AddBlueprint(demonAreaRip, demonRipAreaGuid);

            ///
            var demonRipAuraBuffGuid = new BlueprintGuid(new Guid("0edcbd12-2ed8-4a3b-a1eb-f26d8d473b96"));

            var demonRipAuraBuff = Helpers.Create<BlueprintBuff>(c =>
            {
                c.AssetGuid = demonRipAuraBuffGuid;
                c.Components = new BlueprintComponent[] { };
                c.name = "CarnageBuffAura";
                c.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            });

            demonRipAuraBuff.AddComponent<AddAreaEffect>(c =>
            {
                c.m_AreaEffect = demonAreaRip.ToReference<BlueprintAbilityAreaEffectReference>();
            });

            Helpers.AddBlueprint(demonRipAuraBuff, demonRipAuraBuffGuid);
            ///

            var demonRipGuid = new BlueprintGuid(new Guid("3b571402-e82b-491a-a967-1eb7c77fcda2"));

            var demonRip = Helpers.Create<BlueprintActivatableAbility>(c =>
            {
                c.AssetGuid = demonRipGuid;
                c.name = "Carnage Incarnate" + c.AssetGuid;
                c.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonRip.png");
                c.m_Buff = demonRipAuraBuff.ToReference<BlueprintBuffReference>();
                c.ActivationType = new AbilityActivationType();
                c.DeactivateIfCombatEnded = true;
                c.m_DisplayName = Helpers.CreateString(c + ".Name", "Carnage Incarnate");
            });

            var demonRipDescription = "You let loose a 30ft aura of carnage that causes enemies to die more often in explosive fashion.\n" +
                                                  "Enemies take 1d6 Unholy damage per round and each time an enemy dies while affected by this aura you " +
                                                  "restore a round of rage and they deal 2d6 Unholy damage plus Mythic Rank to all enemies within 10 feet.\n" +
                                                  "Any enemy under this effect will take 1d6 damage for every time they are attacked, hit or miss, at the start of their next round. " +
                                                  "This stacks up to 10 times.\n" +
                                                  "You may use this ability for a number of rounds equal to your mythic rank.";
            demonRip.m_Description = Helpers.CreateString(demonRip + ".Description", demonRipDescription);

            demonRip.AddComponent<ActivatableAbilityResourceLogic>(c =>
            {
                c.SpendType = ActivatableAbilityResourceLogic.ResourceSpendType.NewRound;
                c.m_RequiredResource = demonRipResource.ToReference<BlueprintAbilityResourceReference>();
            });

            Helpers.AddBlueprint(demonRip, demonRipGuid);

            var demonRipFeatureGuid = new BlueprintGuid(new Guid("6f4041fd-6be8-43a8-ae93-5fe1307aba08"));

            var demonRipFeature = Helpers.Create<BlueprintFeature>(c =>
            {
                c.AssetGuid = demonRipFeatureGuid;
                c.m_DisplayName = demonRip.m_DisplayName;
                c.m_Description = demonRip.m_Description;
                c.m_Icon = demonRip.m_Icon;
                c.Ranks = 1;
                c.IsClassFeature = true;
                c.ReapplyOnLevelUp = true;
                c.HideInCharacterSheetAndLevelUp = true;
                c.m_DescriptionShort = new LocalizedString();
                c.Groups = new FeatureGroup[] { };
                c.name = "DemonCarnage";
            });

            demonRipFeature.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[]{
                        demonRip.ToReference<BlueprintUnitFactReference>()
                    };
            });

            demonRipFeature.AddComponent<AddAbilityResources>(c =>
            {
                c.RestoreAmount = true;
                c.m_Resource = demonRipResource.ToReference<BlueprintAbilityResourceReference>();
            });

            Helpers.AddBlueprint(demonRipFeature, demonRipFeatureGuid);

        }
    }
}
