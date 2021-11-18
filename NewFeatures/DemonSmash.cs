using BlueprintCore.Blueprints;
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
using BlueprintCore.Blueprints.Abilities;
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

namespace TabletopTweaks.NewContent.MythicAbilities
{
    class DemonSmash
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            [HarmonyPriority(Priority.First)]
            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;

                Main.Log("Patching Demonic Smash");
                AddDemonSmash();
                AddDemonSmashToMythic();
            }
            public static void AddDemonSmash()
            {
                var fireball00 = BlueprintTool.Get<BlueprintProjectile>("8927afa172e0fc54484a29fa0c4c40c4");
                var fireball = BlueprintTool.Get<BlueprintAbility>("2d81362af43aeac4387a3d4fced489c3");
                var demonChargeMainAbility = BlueprintTool.Get<BlueprintAbility>("1b677ed598d47a048a0f6b4b671b8f84");
                var demonChargeProjectile = BlueprintTool.Get<BlueprintAbility>("4b18d0f44f57cbf4c91f094addfed9f4");
                var telekineticFist = BlueprintTool.Get<BlueprintAbility>("810992c76efdde84db707a0444cf9a1c");

                var demonSmashProGuid = new BlueprintGuid(new Guid("fec53329-817f-4aa6-a921-0be9867a8930"));

                var demonSmashProjectile = Helpers.CreateCopy(fireball00, bp =>
                {
                    bp.AssetGuid = demonSmashProGuid;
                });
                demonSmashProjectile.ProjectileHit.HitFx = demonChargeProjectile.GetComponent<AbilitySpawnFx>().PrefabLink;
                demonSmashProjectile.ProjectileHit.MissFx = demonChargeProjectile.GetComponent<AbilitySpawnFx>().PrefabLink;

                Helpers.AddBlueprint(demonSmashProjectile, demonSmashProGuid);
                Main.Log("Demonic Smash Added " + demonSmashProjectile.AssetGuid.ToString());

                var demonSmashGuid = new BlueprintGuid(new Guid("3013d646-3fa3-4ff8-bb46-eba61b6ed581"));

                BlueprintAbility demonSmash = new();

                demonSmash.AssetGuid = demonSmashGuid;
                demonSmash.name = "Demon Smash";
                demonSmash.m_DisplayName = Helpers.CreateString(demonSmash + ".Name", "Demon Smash");
                demonSmash.m_Description = Helpers.CreateString(demonSmash + ".Description", "This is\n a description.");

                demonSmash.Range = AbilityRange.Weapon;
                demonSmash.Animation = CastAnimationStyle.CoupDeGrace; // Change to Coup De Grace
                demonSmash.CanTargetPoint = false;
                demonSmash.CanTargetSelf = false;
                demonSmash.SpellResistance = false;
                demonSmash.CanTargetEnemies = true;
                demonSmash.CanTargetFriends = true;
                demonSmash.Type = AbilityType.Supernatural;
                demonSmash.m_Icon = telekineticFist.m_Icon;
                demonSmash.HasFastAnimation = true;
                demonSmash.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                demonSmash.ResourceAssetIds = new string[0];
                demonSmash.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                demonSmash.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                demonSmash.LocalizedDuration = new LocalizedString();
                demonSmash.LocalizedSavingThrow = Helpers.CreateString(demonSmash + ".SavingThrow", "None");

                demonSmash.AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = AbilityRankType.DamageDice;
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_StepLevel = 3;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                var fireballAbilityTargetsAround = fireball.GetComponent<AbilityTargetsAround>();

                demonSmash.AddComponent<AbilityTargetsAround>(c =>
                {
                    c.m_Condition = fireballAbilityTargetsAround.m_Condition;
                    c.m_Flags = fireballAbilityTargetsAround.m_Flags;
                    c.m_IncludeDead = fireballAbilityTargetsAround.m_IncludeDead;
                    c.m_PrototypeLink = fireballAbilityTargetsAround.m_PrototypeLink;
                    c.m_Radius = new Kingmaker.Utility.Feet() { m_Value = 5 };
                    c.m_SpreadSpeed = fireballAbilityTargetsAround.m_SpreadSpeed;
                    c.m_TargetType = fireballAbilityTargetsAround.m_TargetType;
                    c.name = fireballAbilityTargetsAround.name;
                    c.m_Condition = new ConditionsChecker()
                    {
                        Conditions = new Condition[0]
                    };
                });

                var demonSmashDamageUnholy = Helpers.Create<ContextActionDealDamage>(c =>
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
                            ValueRank = AbilityRankType.DamageDice
                        },
                        BonusValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            ValueRank = AbilityRankType.DamageDice
                        }
                    };
                    c.IsAoE = true;
                });
                var demonSmashDamageFire = Helpers.Create<ContextActionDealDamage>(c =>
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
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.DamageDice
                        },
                        BonusValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            ValueRank = AbilityRankType.DamageDice
                        }
                    };
                    c.IsAoE = true;
                });

                var demonSmashFx = Helpers.Create<ContextActionProjectileFx>(c =>
                {
                    c.m_Projectile = demonSmashProjectile.ToReference<BlueprintProjectileReference>();
                });
                var demonSmashFx2 = Helpers.Create<ContextActionProjectileFx>(c =>
                {
                    c.m_Projectile = fireball00.ToReference<BlueprintProjectileReference>();
                });

                var conditionalEffects = new Conditional()
                {
                    ConditionsChecker = new ConditionsChecker()
                    {
                        Conditions = new Condition[] {
                            new ContextConditionIsMainTarget() {
                                Not = false
                            }
                        }
                    },
                    IfTrue = new ActionList()
                    {
                        Actions = new GameAction[] { demonSmashFx, demonSmashFx2 }
                    },
                    IfFalse = new ActionList()
                };

                BlueprintAbilityResourceReference demonRageResource = BlueprintTool.Get<BlueprintAbilityResource>("f3bf174f0f86b4f45a823e9ed6ccc7a5").ToReference<BlueprintAbilityResourceReference>();

                var contextResourceIncrease = Helpers.Create<ContextActionIncreaseRageRounds>(c =>
                {
                    c.m_resource = demonRageResource;
                    c.m_resourceAmount = 1;
                });

                var contextMeleeAction = Helpers.Create<ContextActionMeleeAttack>();

                var conditionalDamage = new Conditional()
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
                        Actions = new GameAction[] { contextResourceIncrease, demonSmashDamageUnholy, demonSmashDamageFire }
                    },
                    IfFalse = new ActionList()
                };

                demonSmash.AddComponent<AbilityEffectRunAction>(c =>
                {
                    c.Actions = new ActionList();
                    c.Actions.Actions = new GameAction[] { conditionalDamage, conditionalEffects };
                    c.SavingThrowType = Kingmaker.EntitySystem.Stats.SavingThrowType.Unknown;
                });

                Helpers.AddBlueprint(demonSmash, demonSmashGuid);

                Main.Log("Demonic Smash Added " + demonSmash.AssetGuid.ToString());
            }

            public static void AddDemonSmashToMythic()
            {/*
                var demonSmash = BlueprintTool.Get<BlueprintAbility>("3013d6463fa34ff8bb46eba61b6ed581");

                var demonRageFeature = BlueprintTool.Get<BlueprintFeature>("6a8af3f208a0fa747a465b70b7043019");
                demonRageFeature.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                    demonSmash.ToReference<BlueprintUnitFactReference>()
                };
                });

                Main.Log("Demonic Smash Added To Mythic");
                */
            }
        }
    }

}
