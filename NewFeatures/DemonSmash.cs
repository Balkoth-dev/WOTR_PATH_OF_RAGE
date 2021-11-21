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
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using WOTR_PATH_OF_RAGE.MechanicsChanges;

namespace WOTR_PATH_OF_RAGE.NewFeatures
{
    class DemonSmash
    {
        public static void AddDemonSmash()
        {
            var fireball00 = BlueprintTool.Get<BlueprintProjectile>("8927afa172e0fc54484a29fa0c4c40c4");
            var fireball = BlueprintTool.Get<BlueprintAbility>("2d81362af43aeac4387a3d4fced489c3");
            var demonChargeMainAbility = BlueprintTool.Get<BlueprintAbility>("1b677ed598d47a048a0f6b4b671b8f84");
            var demonChargeProjectile = BlueprintTool.Get<BlueprintAbility>("4b18d0f44f57cbf4c91f094addfed9f4");
            var telekineticFist = BlueprintTool.Get<BlueprintAbility>("810992c76efdde84db707a0444cf9a1c");
            var touchItem = BlueprintTool.Get<BlueprintItemWeapon>("bb337517547de1a4189518d404ec49d4");
            var demonSmashResource = BlueprintTool.Get<BlueprintAbilityResource>("40536705671e4e96979a10a41ea6057e");

            var demonSmashProGuid = new BlueprintGuid(new Guid("fec53329-817f-4aa6-a921-0be9867a8930"));

            var demonSmashProjectile = Helpers.CreateCopy(fireball00, bp =>
            {
                bp.AssetGuid = demonSmashProGuid;
            });
            demonSmashProjectile.ProjectileHit.HitFx = demonChargeProjectile.GetComponent<AbilitySpawnFx>().PrefabLink;
            demonSmashProjectile.ProjectileHit.MissFx = demonChargeProjectile.GetComponent<AbilitySpawnFx>().PrefabLink;

            Helpers.AddBlueprint(demonSmashProjectile, demonSmashProGuid);
            Main.Log("Demonic Projectile Added " + demonSmashProjectile.AssetGuid.ToString());

            var demonSmashGuid = new BlueprintGuid(new Guid("3013d646-3fa3-4ff8-bb46-eba61b6ed581"));

            BlueprintAbility demonSmash = new();

            demonSmash.AssetGuid = demonSmashGuid;
            demonSmash.name = "Demon Smash";
            demonSmash.m_DisplayName = Helpers.CreateString(demonSmash + ".Name", "DEMONIC SMASH!");
            var demonSmashDescription = "You erupt into a frenzy and smash your melee weapon into a foe with explosive results.\n" +
                "You deal normal weapon damage on a hit, and regardless if you hit or miss you deal {g|Encyclopedia:Dice}1d6{/g} " +
                "{g|Encyclopedia:Energy_Damage}unholy{/g} and {g|Encyclopedia:Dice}1d6{/g} {g|Encyclopedia:Energy_Damage}fire{/g} " +
                "{g|Encyclopedia:Damage}damage{/g} per mythic rank in a five foot radius to all targets.\n" +
                "For each target affected you regain one round of Demon Rage. \n" +
                "You may do this 2 times a day with an additional time every mythic rank.";
            demonSmash.m_Description = Helpers.CreateString(demonSmash + ".Description", demonSmashDescription);


            demonSmash.Range = AbilityRange.Weapon;
            demonSmash.Animation = CastAnimationStyle.CoupDeGrace;
            demonSmash.CanTargetPoint = false;
            demonSmash.CanTargetSelf = false;
            demonSmash.SpellResistance = false;
            demonSmash.CanTargetEnemies = true;
            demonSmash.CanTargetFriends = true;
            demonSmash.Type = AbilityType.Supernatural;
            demonSmash.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonSmash.png");
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
                c.HalfIfSaved = true;
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

            var contextMeleeAction = Helpers.Create<ContextActionMeleeAttack>();

            demonSmash.AddComponent<AbilityEffectRunActionOnClickedTarget>(c =>
            {
                c.Action = new ActionList();
                c.Action.Actions = new GameAction[] { contextMeleeAction };
            });

            demonSmash.AddComponent<AbilityEffectRunAction>(c =>
            {
                c.Actions = new ActionList();
                c.Actions.Actions = new GameAction[] { conditionalDamage, conditionalEffects };
            });

            demonSmash.AddComponent<AbilityResourceLogic>(c =>
            {
                c.Amount = 1;
                c.m_IsSpendResource = true;
                c.m_RequiredResource = demonSmashResource.ToReference<BlueprintAbilityResourceReference>();
            });

            demonSmash.AddComponent<AbilityCasterMainWeaponIsMelee>();

            Helpers.AddBlueprint(demonSmash, demonSmashGuid);

            Main.Log("Demonic Smash Added " + demonSmash.AssetGuid.ToString());

            var demonSmashFeatureGuid = new BlueprintGuid(new Guid("23d79963-86d6-4d67-a83e-79f5bc5fedaf"));

            var demonSmashFeature = Helpers.Create<BlueprintFeature>(c =>
            {
                c.AssetGuid = demonSmashFeatureGuid;
                c.m_DisplayName = demonSmash.m_DisplayName;
                c.m_Description = demonSmash.m_Description;
                c.m_Icon = demonSmash.m_Icon;
                c.Ranks = 1;
                c.IsClassFeature = true;
                c.ReapplyOnLevelUp = true;
                c.HideInCharacterSheetAndLevelUp = true;
                c.m_DescriptionShort = new LocalizedString();
                c.Groups = new FeatureGroup[] { };
            });

            demonSmashFeature.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[]{
                        demonSmash.ToReference<BlueprintUnitFactReference>()
                    };
            });

            demonSmashFeature.AddComponent<AddAbilityResources>(c =>
            {
                c.RestoreAmount = true;
                c.m_Resource = demonSmashResource.ToReference<BlueprintAbilityResourceReference>();
            });

            Helpers.AddBlueprint(demonSmashFeature, demonSmashFeatureGuid);
            Main.Log("Demon Smash Feature Added " + demonSmashFeature.AssetGuid.ToString());

        }

    }
}
