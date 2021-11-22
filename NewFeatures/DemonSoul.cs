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
    class DemonSoul
    {
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
            var demonChargeProjectile = BlueprintTool.Get<BlueprintAbility>("4b18d0f44f57cbf4c91f094addfed9f4");

            ///

            var demonSoulBuffGuid = new BlueprintGuid(new Guid("15cfd214-4ae3-4262-9f20-d00f530435a6"));

            var demonSoulBuff = Helpers.CreateCopy(eldritchFontEldritchSurgeDCBuff, bp =>
            {
                bp.AssetGuid = demonSoulBuffGuid;
                bp.m_DisplayName = Helpers.CreateString(bp + ".Name", "Consumed Soul");
                bp.m_Description = Helpers.CreateString(bp + ".Description", "You have consumed a number of souls, you gain a stacking DC bonus to all Demon spells per soul consumed.");
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

            Helpers.AddBlueprint(demonSoulBuff, demonSoulBuffGuid);

            ///

            var demonSoulProjectileGuid = new BlueprintGuid(new Guid("41ad3d7c-303b-4689-b029-d0163fc8a88b"));

            var demonSoulProjectile = Helpers.CreateCopy(mythic4lvlDemon_MorbidRestoration00, bp =>
            {
                bp.AssetGuid = demonSoulProjectileGuid;
            });

            demonSoulProjectile.View = mythic4lvlDemon_AbyssalChains00_Projectile.View;
            demonSoulProjectile.ProjectileHit.HitFx = demonChargeProjectile.GetComponent<AbilitySpawnFx>().PrefabLink;
            demonSoulProjectile.ProjectileHit.MissFx = demonChargeProjectile.GetComponent<AbilitySpawnFx>().PrefabLink;

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
                LocalizedDuration = new LocalizedString(),
                name = "Consume Soul"
            };
            demonSoul.m_DisplayName = Helpers.CreateString(demonSoul + ".Name", "Consume Souls");
            demonSoul.LocalizedSavingThrow = new LocalizedString();
            demonSoul.m_Description = new LocalizedString();
            var demonSoulDescription = "You consume nearby souls of the recently dead, destroying their bodies and boosting your own abilities.\nWhen you do so, " +
                "all special Demon spells DCs increase by 1 for each soul consumed, max 30. This bonus lasts two minutes with an additional minute every two mythic ranks. " +
                "You restore an round of Demon Rage and a random spell-slot in your (non-merged) Demon spellbook per soul consumed.\nYou may use this ability once per day with an addtional use at 6th and 9th mythic rank.";
            demonSoul.m_Description = Helpers.CreateString(demonSoul + ".Description", demonSoulDescription);

            demonSoul.AddComponent<AbilityTargetsAround>(c =>
            {
                c.m_IncludeDead = true;
                c.m_Radius = new Kingmaker.Utility.Feet() { m_Value = 30 };
                c.m_TargetType = TargetType.Any;
                c.name = "AreaDemonSoulEffect";
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

            var demonSoulFeatureGuid = new BlueprintGuid(new Guid("30dcbf93-9783-418c-881d-15f623d53bf9"));

            var demonSoulFeature = Helpers.Create<BlueprintFeature>(c =>
            {
                c.AssetGuid = demonSoulFeatureGuid;
                c.name = "DemonSoul" + c.AssetGuid;
                c.m_DisplayName = demonSoul.m_DisplayName;
                c.m_Description = demonSoul.m_Description;
                c.m_Icon = demonSoul.m_Icon;
                c.Ranks = 1;
                c.IsClassFeature = true;
                c.ReapplyOnLevelUp = true;
                c.HideInCharacterSheetAndLevelUp = true;
                c.m_DescriptionShort = new LocalizedString();
                c.Groups = new FeatureGroup[] { };
            });

            demonSoulFeature.AddComponent<AddFacts>(c =>
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

    }
}
