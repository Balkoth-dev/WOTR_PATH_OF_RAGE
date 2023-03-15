
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
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Components;
using BlueprintCore.Utils;

namespace WOTR_PATH_OF_RAGE.NewFeatures
{
    class DemonTear
    {
        public static void AddDemonTear()
        {
            var devilClawLeft00 = BlueprintTool.Get<BlueprintProjectile>("11a1193626d322b49b3cf5578384142b").ToReference<BlueprintProjectileReference>();
            var devilClawRight00 = BlueprintTool.Get<BlueprintProjectile>("f76e194520d6f9946bb48d8852ce9e8c").ToReference<BlueprintProjectileReference>();
            var clawType = BlueprintTool.Get<BlueprintWeaponType>("d4f7aee36efe0b54e810c9d3407b6ab3").ToReference<BlueprintWeaponTypeReference>();
            var slamType = BlueprintTool.Get<BlueprintWeaponType>("f18cbcb39a1b35643a8d129b1ec4e716").ToReference<BlueprintWeaponTypeReference>();
            var bleed1d4Buff = BlueprintTool.Get<BlueprintBuff>("5eb68bfe186d71a438d4f85579ce40c1").ToReference<BlueprintBuffReference>();
            var bloodHazeBuff = BlueprintTool.Get<BlueprintBuff>("173af01d6aae5574ba0391e277e9b168");

            var demonTearResource = BlueprintTool.Get<BlueprintAbilityResource>("3c30cf94f6de45a9a79943970fa7a2f5");

            ///
            var demonTearBuffGuid = new BlueprintGuid(new Guid("0ca731b3-293c-4dea-a30c-1a5444e901d6"));

            var demonTearBuff = Helpers.Create<BlueprintBuff>(c =>
            {
                c.AssetGuid = demonTearBuffGuid;
                c.name = "Rend Asunder Buff" + c.AssetGuid;
                c.m_DisplayName = Helpers.CreateString(c + ".Name", "Frenzied");
                c.m_Description = Helpers.CreateString(c + ".Description", "While Frenzied, you gain 1 extra attack at your highest Attack Bonus, this stacks with Haste.\n" +
                                                           "The number of attacks increase by 1 at 6th and 9th Mythic Rank.\n" +
                                                           "This lasts until the end of combat or until you equip a weapon or shield.");
                c.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonTear.png");
                c.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                c.Components = new BlueprintComponent[] { };
                c.FxOnStart = bloodHazeBuff.FxOnStart;
                c.FxOnRemove = bloodHazeBuff.FxOnRemove;
            });

            demonTearBuff.AddComponent<CombatStateTrigger>(c => {
                c.CombatStartActions = new ActionList();
                c.CombatEndActions = new ActionList();
                c.CombatEndActions.Actions = new GameAction[] { Helpers.Create<ContextActionRemoveSelf>() };
            });

            demonTearBuff.AddComponent<BuffExtraAttackDemon>(c => c.Haste = false);

            var conditionalBuffEffects = new Conditional()
            {
                ConditionsChecker = new ConditionsChecker()
                {
                    Conditions = new Condition[] {
                            new ContextConditionHasWeapons() {
                                Category = new WeaponCategory[] { WeaponCategory.Claw }
                            },
                            new ContextConditionIsInCombat(){}
                        }
                },
                IfTrue = new ActionList(),
                IfFalse = new ActionList() { Actions = new GameAction[] { Helpers.Create<ContextActionRemoveSelf>() } }
            };

            demonTearBuff.AddComponent<NewRoundTrigger>(c => {
                c.NewRoundActions = new ActionList();
                c.NewRoundActions.Actions = new GameAction[]{ conditionalBuffEffects };
                });

            Helpers.AddBlueprint(demonTearBuff, demonTearBuffGuid);

            ///

            var demonTearAbilityGuid = new BlueprintGuid(new Guid("766217fd-9e3d-4138-8d4f-ac3502598afb"));

            var demonTearAbility = Helpers.Create<BlueprintAbility>(c =>
            {
                c.AssetGuid = demonTearAbilityGuid;
                c.name = "Frenzy" + c.AssetGuid;
                c.m_DisplayName = Helpers.CreateString(c + ".Name", "Frenzy");
                c.m_Description = Helpers.CreateString(c + ".Description", "During combat you can go into a frenzy so long as you are only using claws, gaining an extra attack that stacks with haste. " +
                                                                           "The number of attacks increase by 1 at 6th and 9th Mythic Rank.\n" +
                                                                           "You may only use this ability when using claws in both hands. " +
                                                                           "This ability lasts until the end of combat or until you equip a weapon or shield.\n" +
                                                                           "You may use this ability three times per day, with an additional time at 6th and 9th mythic rank.");
                c.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonTear.png");
                c.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
                c.Animation = CastAnimationStyle.Immediate;
                c.CanTargetSelf = true;
                c.Range = AbilityRange.Personal;
                c.Type = AbilityType.Supernatural;
                c.ResourceAssetIds = new string[0];
                c.LocalizedDuration = new LocalizedString();
                c.LocalizedSavingThrow = Helpers.CreateString(c + ".SavingThrow", "None");
            });

            demonTearAbility.AddComponent<AbilityResourceLogic>(c =>
            {
                c.Amount = 1;
                c.m_IsSpendResource = true;
                c.m_RequiredResource = demonTearResource.ToReference<BlueprintAbilityResourceReference>();
            });

            demonTearAbility.AddComponent<AbilityCasterInCombat>();
            
            demonTearAbility.AddComponent<AbilityEffectRunAction>(c => {
                c.Actions = new ActionList();
                c.Actions.Actions = new GameAction[] { Helpers.Create<ContextActionApplyBuff>(c => {
                    c.m_Buff = demonTearBuff.ToReference<BlueprintBuffReference>();
                    c.Permanent = true;
                    c.DurationValue = Helpers.Create<ContextDurationValue>(c => { c.m_IsExtendable = true; });})};
            });
            demonTearAbility.AddComponent<AbilityCasterMainWeaponCheck>(c => {
                c.Category = new WeaponCategory[] { WeaponCategory.Claw, WeaponCategory.Slam };                 
            });
            demonTearAbility.AddComponent<AbilityCasterSecondaryWeaponCheck>(c => {
                c.Category = new WeaponCategory[] { WeaponCategory.Claw, WeaponCategory.Slam };
            });
            demonTearAbility.AddComponent<AbilityCasterNotPolymorphed>();

            demonTearAbility.AddComponent<AbilityCasterHasNoFacts>(c => c.m_Facts = new BlueprintUnitFactReference[]{
                        demonTearBuff.ToReference<BlueprintUnitFactReference>()
                    });
            
            
            Helpers.AddBlueprint(demonTearAbility, demonTearAbilityGuid);

            ///

            var demonTearGuid = new BlueprintGuid(new Guid("6fc3b519-1853-4144-9e4a-4fd803b34d35"));

            var demonTearFeatureGuid = new BlueprintGuid(new Guid("b044212d-111b-4e26-8653-44982084e5c7"));
            var demonTearDescription = "They are rage, brutal, without mercy. But you. You will be worse. Rend Asunder, until it is done.\n" +
                                       "Your claws deal 1d4 bleeding damage and you gain the Rend ability so long as you only have claws equipped, dealing 2d8 per Mythic Rank Unholy damage when both claw attacks hit. " +
                                       "Whenever you successfully Rend, you have a 50% chance of gaining a round of Demon Rage.\n" +
                                       "In addition you gain the ability to frenzy, gaining additional attacks that stack with Haste as long as you wield only claws.\n" +
                                       "This bonus does not apply when polymorphed.";
            var demonTearFeature = Helpers.Create<BlueprintFeature>(c =>
            {
                c.AssetGuid = demonTearFeatureGuid;
                c.m_DisplayName = Helpers.CreateString(c + ".Name", "Rend Asunder");
                c.m_Description = Helpers.CreateString(c + ".Description", demonTearDescription);
                c.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonTear.png");
                c.Ranks = 1;
                c.IsClassFeature = true;
                c.ReapplyOnLevelUp = true;
                c.HideInCharacterSheetAndLevelUp = true;
                c.m_DescriptionShort = new LocalizedString();
                c.Groups = new FeatureGroup[] { };
                c.name = "Rend Asunder"+c.AssetGuid;
            });

            var demonTearRightFx = Helpers.Create<ContextActionProjectileFx>(c =>
            {
                c.m_Projectile = devilClawRight00;                
            });
            var demonTearLeftFx = Helpers.Create<ContextActionProjectileFx>(c =>
            {
                c.m_Projectile = devilClawLeft00;
            });

            BlueprintAbilityResourceReference demonRageResource = BlueprintTool.Get<BlueprintAbilityResource>("bc2c2f64ada54c78a250f8b72c48ae57").ToReference<BlueprintAbilityResourceReference>();

            var contextResourceIncrease = Helpers.Create<ContextActionIncreaseRageRounds>(c =>
            {
                c.m_resource = demonRageResource;
                c.m_resourceAmount = 1;
                c.m_RandomChance = true;
                c.m_RandomChancePercent = 50;
            });

            var demonTearNotPolymorphed = Helpers.Create<ConditionsChecker>(c =>
            {
                c.Conditions = new Condition[] {
                            new ContextConditionIsShapeshifted() {
                                Not = true
                            }
                        };                
            });

            demonTearFeature.AddComponent<DemonRend>(c =>
            {
                c.RendDamage = new DiceFormula { m_Rolls = 2, m_Dice = DiceType.D8 };
                c.RendType = new DamageTypeDescription { Common = new DamageTypeDescription.CommomData(), Energy = DamageEnergyType.Unholy, Type = DamageType.Energy };
                c.SpellDescriptor = SpellDescriptor.Polymorph;
                c.CheckBuff = false;
                c.CheckShapeshift = true;
                c.Action = new ActionList();
                c.Category = new WeaponCategory[] { WeaponCategory.Claw, WeaponCategory.Slam };
                c.Action.Actions = new GameAction[] { contextResourceIncrease, demonTearRightFx, demonTearLeftFx };
                c.UseMythicLevel = true;
                c.ApplyStrengthBonus = false;
            });

            var contextActionBleed = Helpers.Create<ContextActionApplyBuff>(c => {
                c.m_Buff = bleed1d4Buff;
                c.DurationSeconds = 18;
                c.UseDurationSeconds = true;
                c.DurationValue = new ContextDurationValue();
                c.IsFromSpell = true;
            });

            var conditionalEffects = Helpers.Create<Conditional>(c =>
            {
                c.ConditionsChecker = demonTearNotPolymorphed;
                c.IfTrue = new ActionList()
                {
                    Actions = new GameAction[] { demonTearRightFx, contextActionBleed }
                };
                c.IfFalse = new ActionList();
            });

            demonTearFeature.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
            {
                c.OnlyHit = true;
                c.Action = new ActionList();
                c.Action.Actions = new GameAction[] { conditionalEffects };
                c.m_WeaponType = clawType;
            });

            demonTearFeature.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
            {
                c.OnlyHit = true;
                c.Action = new ActionList();
                c.Action.Actions = new GameAction[] { conditionalEffects };
                c.m_WeaponType = slamType;
            });

            demonTearFeature.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[]{
                        demonTearAbility.ToReference<BlueprintUnitFactReference>()
                    };
            });

            demonTearFeature.AddComponent<AddAbilityResources>(c =>
            {
                c.RestoreAmount = true;
                c.m_Resource = demonTearResource.ToReference<BlueprintAbilityResourceReference>();
            });

            Helpers.AddBlueprint(demonTearFeature, demonTearFeatureGuid);

            Main.Log("Demon Tear Feature Created" + demonTearFeatureGuid);


        }

    }
}
