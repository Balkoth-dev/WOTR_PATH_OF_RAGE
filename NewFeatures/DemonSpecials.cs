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

namespace TabletopTweaks.NewContent.MythicAbilities
{
    class DemonSpecials
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
                AddDemonSpecialFeatures();
                AddDemonSpecialResources();
                AddDemonSmash();
                AddDemonSoul();
                AddDemonSpecialsToSelection();
                AddDemonSpecialSelectionToMythic();
                AddDemonBlast();
            }
            public static void AddDemonSpecialFeatures()
            {
                var demonAspectSelection = BlueprintTool.Get<BlueprintFeatureSelection>("bbfc0d06955db514ba23337c7bf2cca6");
                var demonSpecialsGuid = new BlueprintGuid(new Guid("1df9edd3-e5f4-4857-93e5-7a40e1d567f2"));

                var demonSpecialSelection = Helpers.CreateCopy(demonAspectSelection, bp =>
                {
                    bp.AssetGuid = demonSpecialsGuid;
                    bp.m_AllFeatures = new BlueprintFeatureReference[] { };
                    bp.m_DisplayName = Helpers.CreateString(bp + ".Name", "Demonologies");
                    bp.m_Description = Helpers.CreateString(bp + ".Description", "Special abilties for controlling your rage.");
                });

                Helpers.AddBlueprint(demonSpecialSelection, demonSpecialsGuid);
                Main.Log("Demon Specials Selection Added " + demonSpecialSelection.AssetGuid.ToString());

            }
            public static void AddDemonSpecialResources()
            {
                var demonRageResource = BlueprintTool.Get<BlueprintAbilityResource>("f3bf174f0f86b4f45a823e9ed6ccc7a5");
                var demonSmashResourceGuid = new BlueprintGuid(new Guid("40536705-671e-4e96-979a-10a41ea6057e"));

                var demonSmashResource = Helpers.CreateCopy(demonRageResource, bp =>
                {
                    bp.AssetGuid = demonSmashResourceGuid;
                    bp.m_MaxAmount.BaseValue = 2;
                    bp.m_MaxAmount.StartingLevel = 2;
                    bp.m_MaxAmount.LevelStep = 1;
                    bp.m_MaxAmount.PerStepIncrease = 1;
                });

                Helpers.AddBlueprint(demonSmashResource, demonSmashResourceGuid);

                var demonSoulResourceGuid = new BlueprintGuid(new Guid("0c9c6e72-90a9-412f-ade8-d97291a005e3"));

                var demonSoulResource = Helpers.CreateCopy(demonRageResource, bp =>
                {
                    bp.AssetGuid = demonSoulResourceGuid;
                    bp.m_MaxAmount.BaseValue = 1;
                    bp.m_MaxAmount.StartingLevel = 3;
                    bp.m_MaxAmount.LevelStep = 3;
                    bp.m_MaxAmount.PerStepIncrease = 1;
                });

                Helpers.AddBlueprint(demonSoulResource, demonSoulResourceGuid);

                var demonSuccResourceGuid = new BlueprintGuid(new Guid("fb938b3d-9deb-46b3-b3a4-4de61cd2d574"));

                var demonSuccResource = Helpers.CreateCopy(demonRageResource, bp =>
                {
                    bp.AssetGuid = demonSuccResourceGuid;
                    bp.m_MaxAmount.BaseValue = 2;
                    bp.m_MaxAmount.StartingLevel = 2;
                    bp.m_MaxAmount.LevelStep = 1;
                    bp.m_MaxAmount.PerStepIncrease = 1;
                });

                Helpers.AddBlueprint(demonSuccResource, demonSuccResourceGuid);

            }
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

                var babauAspectFeature = BlueprintTool.Get<BlueprintFeature>("99a34a0fa0c3a154fbc5b11fe2d18009");
                var demonSmashFeatureGuid = new BlueprintGuid(new Guid("23d79963-86d6-4d67-a83e-79f5bc5fedaf"));

                var demonSmashFeature = Helpers.CreateCopy(babauAspectFeature, bp =>
                {
                    bp.AssetGuid = demonSmashFeatureGuid;
                    bp.m_DisplayName = demonSmash.m_DisplayName;
                    bp.m_Description = demonSmash.m_Description;
                    bp.m_Icon = demonSmash.m_Icon;
                });

                demonSmashFeature.EditComponent<AddFacts>(c =>
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

            public static void AddDemonSoul()
            {
                var morbidRestorationHealingAbility = BlueprintTool.Get<BlueprintAbility>("04b0ea02e1db66c44a8c31b0d0badff8");
                var mythic4lvlDemon_AbyssalChains00_Projectile = BlueprintTool.Get<BlueprintProjectile>("125269c9c1898e240a2f70f3b148fd60");
                var mythic4lvlDemon_MorbidRestoration00 = BlueprintTool.Get<BlueprintProjectile>("62b6d54d5b52eba4da3c2fa2acc30958");
                var demonSpelllist = BlueprintTool.Get<BlueprintSpellList>("abb1991bf6e996348bb743471ee7e1c1");
                var eldritchFontEldritchSurgeDCBuff = BlueprintTool.Get<BlueprintBuff>("91b2762997f0d8044baeeef0871eac6f");
                var abyssalChainsBuff = BlueprintTool.Get<BlueprintBuff>("32c0fa6d6b154f06bfdb50bd70096aa8");
                var fireball = BlueprintTool.Get<BlueprintAbility>("2d81362af43aeac4387a3d4fced489c3");
                var demonSoulResource = BlueprintTool.Get<BlueprintAbilityResource>("0c9c6e7290a9412fade8d97291a005e3");
                var demonSpellbook = BlueprintTool.Get<BlueprintSpellbook>("e3daa889c72982e45a026f62cc84937d");

                ///

                var demonSoulBuffGuid = new BlueprintGuid(new Guid("15cfd214-4ae3-4262-9f20-d00f530435a6"));

                var demonSoulBuff = Helpers.CreateCopy(eldritchFontEldritchSurgeDCBuff, bp =>
                {
                    bp.AssetGuid = demonSoulBuffGuid;
                    bp.m_DisplayName = Helpers.CreateString(bp + ".Name", "Consumed Soul");
                    bp.m_Description = Helpers.CreateString(bp + ".Description", "You have consumed a number of souls, you gain +2 to all attacks and a stacking DC bonus to all Demon spells per soul consumed.");
                    bp.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonSoulBuff.png");
                    bp.Components = new BlueprintComponent[] { };
                    bp.Stacking = StackingType.Rank;
                    bp.Ranks = 30;
                });

                demonSoulBuff.AddComponent<IncreaseSpelllistDC>(c =>
                {
                    c.BonusDC = 1;
                    c.m_SpellList = demonSpelllist.ToReference<BlueprintSpellListReference>();
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });

                demonSoulBuff.AddComponent<AddContextStatBonus>(c =>
                {
                    c.Multiplier = 2;
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus;
                    c.Value = 1;
                });

                Helpers.AddBlueprint(demonSoulBuff, demonSoulBuffGuid);

                ///

                var demonSoulProjectileGuid = new BlueprintGuid(new Guid("41ad3d7c-303b-4689-b029-d0163fc8a88b"));

                var demonSoulProjectile = Helpers.CreateCopy(mythic4lvlDemon_MorbidRestoration00, bp =>
                {
                    bp.AssetGuid = demonSoulProjectileGuid;
                });

                demonSoulProjectile.View = mythic4lvlDemon_AbyssalChains00_Projectile.View;

                Helpers.AddBlueprint(demonSoulProjectile, demonSoulProjectileGuid);

                ///

                var demonSoulAbilityGuid = new BlueprintGuid(new Guid("ecf53e56-935c-4a7a-b35c-d4c4496657f2"));

                var demonSoulAbility = Helpers.CreateCopy(morbidRestorationHealingAbility, bp =>
                {
                    bp.AssetGuid = demonSoulAbilityGuid;
                });

                var demonSoulAbilityAbilityEffectRunAction = demonSoulAbility.GetComponent<AbilityEffectRunAction>();

                var demonSoulAbilityAbilityProjectile = (ContextActionProjectileFx)demonSoulAbilityAbilityEffectRunAction.Actions.Actions[1];

                demonSoulAbilityAbilityProjectile.m_Projectile = demonSoulProjectile.ToReference<BlueprintProjectileReference>();

                Helpers.AddBlueprint(demonSoulAbility, demonSoulAbilityGuid);

                ///

                var morbidRestoration = BlueprintTool.Get<BlueprintAbility>("8baeffb9caf081e449ce52bf7bdd6236");

                var demonSoulGuid = new BlueprintGuid(new Guid("ea6ea886-0fe7-470a-b0bf-427e598c494f"));

                var demonSoul = new BlueprintAbility()
                {
                    AssetGuid = demonSoulGuid,
                    Range = AbilityRange.Personal,
                    ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift,
                    m_Icon = AssetLoader.LoadInternal("Abilities", "DemonSoul.png"),
                    Animation = CastAnimationStyle.Immediate,
                    CanTargetPoint = true,
                    CanTargetSelf = true,
                    CanTargetEnemies = true,
                    CanTargetFriends = true,
                    Type = AbilityType.Supernatural,
                    HasFastAnimation = true,
                    ResourceAssetIds = new string[0],
                    EffectOnEnemy = AbilityEffectOnUnit.Harmful,
                    EffectOnAlly = AbilityEffectOnUnit.Harmful,
                    LocalizedDuration = new LocalizedString()
                };
                demonSoul.m_DisplayName = Helpers.CreateString(demonSoul + ".Name", "Consume Souls");
                demonSoul.LocalizedSavingThrow = new LocalizedString();
                demonSoul.m_Description = new LocalizedString();
                   var demonSoulDescription = "You consume nearby souls of the recently dead, destroying their bodies and boosting your own abilities.\nWhen you do so, " +
                       "you gain a +2 attack bonus. In addition all special Demon spells DCs increase by 1 for each soul consumed, max 30. This bonus lasts two minutes with an additional minute every two mythic ranks. " +
                       "You restore an round of Demon Rage and a random spell-slot in your (non-merged) Demon spellbook per soul consumed.\nYou may use this ability once per day with an addtional use at 6th and 9th mythic rank.";
                   demonSoul.m_Description = Helpers.CreateString(demonSoul + ".Description", demonSoulDescription);
                
                var fireballAbilityTargetsAround = fireball.GetComponent<AbilityTargetsAround>();
                demonSoul.AddComponent<AbilityTargetsAround>(c =>
                {
                    c.m_IncludeDead = true;
                    c.m_Radius = new Kingmaker.Utility.Feet() { m_Value = 30 };
                    c.m_TargetType = TargetType.Any;
                    c.name = fireballAbilityTargetsAround.name;
                    c.m_Condition = new ConditionsChecker()
                    {
                        Conditions = new Condition[] {
                            new ContextConditionIsCaster() {
                                Not = true
                            },
                            new ContextConditionAlive() {
                                Not = true  }
                        }
                    };
                });

                demonSoul.AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_StepLevel = 2;
                    c.m_Progression = ContextRankProgression.OnePlusDiv2;
                });

                var contextApplyBuff = new ContextActionApplyBuff()
                {
                    m_Buff = demonSoulBuff.ToReference<BlueprintBuffReference>(),
                    DurationValue = new ContextDurationValue()
                    {
                        m_IsExtendable = true,
                        Rate = DurationRate.Minutes,
                        BonusValue = new ContextValue() { ValueType = ContextValueType.Rank, ValueRank = AbilityRankType.StatBonus },
                        DiceType = DiceType.Zero,
                        DiceCountValue = 0
                    },
                    ToCaster = true,
                    IsFromSpell = true
                };
                
                BlueprintAbilityResourceReference demonRageResource = BlueprintTool.Get<BlueprintAbilityResource>("f3bf174f0f86b4f45a823e9ed6ccc7a5").ToReference<BlueprintAbilityResourceReference>();

                var contextResourceIncrease = Helpers.Create<ContextActionIncreaseRageRounds>(c =>
                {
                    c.m_resource = demonRageResource;
                    c.m_resourceAmount = 1;
                });

                var demonSoulAbilityComponents = demonSoulAbility.GetComponent<AbilityEffectRunAction>().Actions.Actions;


                var demonSoulSpellSlotRestore = Helpers.Create<ContextActionRestoreRandomSpell>(c =>
                {
                    c.m_Spellbook = demonSpellbook.ToReference<BlueprintSpellbookReference>();
                    c.m_Amount = 1;
                });

                demonSoul.AddComponent<AbilityEffectRunAction>(c =>
                {
                    c.Actions = new ActionList();
                    c.Actions.Actions = new GameAction[] { contextApplyBuff, demonSoulAbilityComponents[1], contextResourceIncrease, demonSoulSpellSlotRestore };
                });

                demonSoul.AddComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 1;
                    c.m_IsSpendResource = true;
                    c.m_RequiredResource = demonSoulResource.ToReference<BlueprintAbilityResourceReference>();
                });

                Helpers.AddBlueprint(demonSoul, demonSoulGuid);
                Main.Log("Consume Souls Added " + demonSoul.AssetGuid.ToString());

                var babauAspectFeature = BlueprintTool.Get<BlueprintFeature>("99a34a0fa0c3a154fbc5b11fe2d18009");
                var demonSoulFeatureGuid = new BlueprintGuid(new Guid("30dcbf93-9783-418c-881d-15f623d53bf9"));

                var demonSoulFeature = Helpers.CreateCopy(babauAspectFeature, bp =>
                {
                    bp.AssetGuid = demonSoulFeatureGuid;
                    bp.m_DisplayName = demonSoul.m_DisplayName;
                    bp.m_Description = demonSoul.m_Description;
                    bp.m_Icon = demonSoul.m_Icon;
                });

                demonSoulFeature.EditComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[]{
                        demonSoul.ToReference<BlueprintUnitFactReference>()
                    };
                });

                demonSoulFeature.AddComponent<AddAbilityResources>(c =>
                {
                    c.RestoreAmount = true;
                    c.m_Resource = demonSoulResource.ToReference<BlueprintAbilityResourceReference>();
                });

                Helpers.AddBlueprint(demonSoulFeature, demonSoulFeatureGuid);
                Main.Log("Consume Souls Feature Added " + demonSoulFeature.AssetGuid.ToString());

            }

            public static void AddDemonBlast()
            {
                if(Main.settings.AddDemonBlast == false)
                {
                    return;
                }

                var demonChargeMainAbility = BlueprintTool.Get<BlueprintAbility>("1b677ed598d47a048a0f6b4b671b8f84");

                var demonBlastGuid = new BlueprintGuid(new Guid("6fc3b519-1853-4144-9e4a-4fd803b34d35"));

                var demonBlast = Helpers.CreateCopy(demonChargeMainAbility, bp =>
                {
                    bp.AssetGuid = demonBlastGuid;
                    bp.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonBlast.png");
                });
                demonBlast.m_DisplayName = Helpers.CreateString(demonBlast + ".Name", "Demonic Blast");
                demonBlast.LocalizedSavingThrow = Helpers.CreateString(demonBlast + ".SavingThrow", "None");
                var demonSoulDescription = "As a {g|Encyclopedia:Move_Action}move action{/g}, you can let loose an explosion, dealing {g|Encyclopedia:Dice}2d6{/g} unholy {g|Encyclopedia:Damage}damage{/g} " +
                    "per mythic rank to all enemies in a 10 feet range.\n An enemy can only be damaged by this ability or Demonic Charge once per {g|Encyclopedia:Combat_Round}round{/g}.";
                demonBlast.m_Description = Helpers.CreateString(demonBlast + ".Description", demonSoulDescription);

                demonBlast.RemoveComponents<AbilityCustomTeleportation>();

                var babauAspectFeature = BlueprintTool.Get<BlueprintFeature>("99a34a0fa0c3a154fbc5b11fe2d18009");
                var demonSoulFeatureGuid = new BlueprintGuid(new Guid("6cf0d55c-050c-497a-8b98-e245435ce6aa"));

                var demonBlastFeature = Helpers.CreateCopy(babauAspectFeature, bp =>
                {
                    bp.AssetGuid = demonSoulFeatureGuid;
                    bp.m_DisplayName = demonBlast.m_DisplayName;
                    bp.m_Description = demonBlast.m_Description;
                    bp.m_Icon = demonBlast.m_Icon;
                });

                demonBlastFeature.EditComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[]{
                        demonBlast.ToReference<BlueprintUnitFactReference>()
                    };
                });

                var demonProgression = BlueprintTool.Get<BlueprintProgression>("285fe49f7df8587468f676aa49362213");

                demonProgression.LevelEntries[1].m_Features.Add(demonBlastFeature.ToReference<BlueprintFeatureBaseReference>());

            }

            public static void AddDemonSpecialsToSelection()
            {
                var demonSpecialSelection = BlueprintTool.Get<BlueprintFeatureSelection>("1df9edd3e5f4485793e57a40e1d567f2");

                var demonSmashFeature = BlueprintTool.Get<BlueprintFeature>("23d7996386d64d67a83e79f5bc5fedaf");
                var demonSoulFeature = BlueprintTool.Get<BlueprintFeature>("30dcbf939783418c881d15f623d53bf9");

                demonSpecialSelection.SetFeatures(demonSmashFeature, demonSoulFeature);

                Main.Log("Demonic Specials Added To Selection");

            }

            public static void AddDemonSpecialSelectionToMythic()
            {
                if (Main.settings.AddDemonSpecialSelectionToMythic == false)
                {
                    return;
                }
                var demonSpecialSelection = BlueprintTool.Get<BlueprintFeatureSelection>("1df9edd3e5f4485793e57a40e1d567f2");

                var demonProgression = BlueprintTool.Get<BlueprintProgression>("285fe49f7df8587468f676aa49362213");

                demonProgression.LevelEntries[0].m_Features.Add(demonSpecialSelection.ToReference<BlueprintFeatureBaseReference>());

                Main.Log("Demonic Specials Added To Mythic");
            }
        }
    }

}
