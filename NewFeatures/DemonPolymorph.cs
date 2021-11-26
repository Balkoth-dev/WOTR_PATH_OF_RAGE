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
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Buffs;
using System.Collections.Generic;

namespace WOTR_PATH_OF_RAGE.NewFeatures
{
    class DemonPolymorph
    {
        public static void AddDemonPolymorph()
        {
            if (Main.settings.AddDemonBlast == false)
            {
                return;
            }

            BlueprintAbilityResourceReference demonRageResource = BlueprintTool.Get<BlueprintAbilityResource>("f3bf174f0f86b4f45a823e9ed6ccc7a5").ToReference<BlueprintAbilityResourceReference>();

            var demonicFormISchirBuff = BlueprintTool.Get<BlueprintBuff>("cfb58f71515d6fd49893a10de7984a43");
            var demonicFormIVGalluBuff = BlueprintTool.Get<BlueprintBuff>("051c8dea7acf6aa41b8b1c1f65cda421");
            var demonicFormIBabauBuff = BlueprintTool.Get<BlueprintBuff>("6b1bb8f9879c15e48a3a696d4b221f24");
            var demonicFormIBrimorakBuff = BlueprintTool.Get<BlueprintBuff>("5134534352b09884fb0495c36585aabc");
            var demonicFormIIIVrockBuff = BlueprintTool.Get<BlueprintBuff>("f1a47ec9041f17147adfc17156e05822");
            var demonicFormIIDerakniBuff = BlueprintTool.Get<BlueprintBuff>("b6048f8b7b1598141a66c99df1011eee");
            var demonicFormIVVavakiaBuff = BlueprintTool.Get<BlueprintBuff>("13c8e843d01eef5479efcd6a9adac432");
            var demonicFormIVBalorBuff = BlueprintTool.Get<BlueprintBuff>("e1c5725668f48df48a9676d26aa57fbf");
            var demonicFormIVMarilithBuff = BlueprintTool.Get<BlueprintBuff>("f048ee68bc72da447970025667a77b12");
            var demonicFormIIKalavakusBuff = BlueprintTool.Get<BlueprintBuff>("2814792301df65b4e866acaac9864256");
            var demonicFormIIINalfeshneeBuff = BlueprintTool.Get<BlueprintBuff>("469a412c607bf4f43aabe62c2de22837");
            var demonicFormIINabasuBuff = BlueprintTool.Get<BlueprintBuff>("82d638a78c1a7704684555189ba85d88");
            var demonicFormIIIGlabrezuBuff = BlueprintTool.Get<BlueprintBuff>("f65726a206c68af4085af036f58aca45");
            var keen = BlueprintTool.Get<BlueprintWeaponEnchantment>("102a9c8c9b7a75e4fb5844e79deaf4c0").ToReference<BlueprintFeatureReference>();

            var demonPolyResource = BlueprintTool.Get<BlueprintAbilityResource>("fb938b3d9deb46b3b3a44de61cd2d574");

            var sW_GlabrezuBoss = BlueprintTool.Get<BlueprintUnit>("2433f902e6d443e2a6010de9e4227e9e");

            var bite2d6 = BlueprintTool.Get<BlueprintItemWeapon>("2abc1dc6172759c42971bd04b8c115cb").ToReference<BlueprintItemWeaponReference>();
            var claw1d6 = BlueprintTool.Get<BlueprintItemWeapon>("65eb73689b94d894080d33a768cdf645").ToReference<BlueprintItemWeaponReference>();
            var sting1d4 = BlueprintTool.Get<BlueprintItemWeapon>("df44800dbe7b4ba43ac6e0e435041ed8").ToReference<BlueprintItemWeaponReference>();

            ///
            var demonicFormIVBalor = BlueprintTool.Get<BlueprintAbility>("0258a875670fa134590c6ffdc23da2cf");
            var DemonicFormIIIGlabrezu = BlueprintTool.Get<BlueprintAbility>("0258a875670fa134590c6ffdc23da2cf");

            var demonBalorFormGuid = new BlueprintGuid(new Guid("e30768ff-138f-4ab5-b947-03df2aa72a6a"));

            var demonBalorForm = Helpers.CreateCopy(demonicFormIVBalor, bp =>
            {
                bp.AssetGuid = demonBalorFormGuid;
                bp.name = "Balor Transformation";
            });

            demonBalorForm.RemoveComponents<ContextRankConfig>();

            var demonBalorFormContextActionApplyBuff = (ContextActionApplyBuff)demonBalorForm.GetComponent<AbilityEffectRunAction>().Actions.Actions[1];
            demonBalorFormContextActionApplyBuff.DurationValue = null;
            demonBalorFormContextActionApplyBuff.DurationSeconds = 60;
            demonBalorFormContextActionApplyBuff.UseDurationSeconds = true;

            demonBalorForm.AddComponent<AbilityResourceLogic>(c =>
            {
                c.Amount = 1;
                c.m_IsSpendResource = true;
                c.m_RequiredResource = demonPolyResource.ToReference<BlueprintAbilityResourceReference>();
            });

            Helpers.AddBlueprint(demonBalorForm, demonBalorFormGuid);

            Main.Log("Demon Balor Form Added" + demonBalorForm.AssetGuid.ToString());
            ///

            var demonSinGuzzlerBuffGuid = new BlueprintGuid(new Guid("e0edd270-bb50-4b45-937f-b60d047e0fd5"));

            var demonSinGuzzlerBuff = Helpers.CreateCopy(demonicFormIIIGlabrezuBuff, bp =>
            {
                bp.AssetGuid = demonSinGuzzlerBuffGuid;
                bp.name = "Demonic Form Sin Guzzler"+ bp.AssetGuid;
                bp.m_Icon = DemonicFormIIIGlabrezu.m_Icon;
            });
            demonSinGuzzlerBuff.m_DisplayName = Helpers.CreateString(demonSinGuzzlerBuff + ".Name", "Demonic Form - Sin Guzzler");
            var demonSinGuzzlerBuffDescription = "You transform into a Sin Guzzler. Gaining the Rend of the Glabrezu, DR10/Good, and are considered airborne.";
            demonSinGuzzlerBuff.m_Description = Helpers.CreateString(demonSinGuzzlerBuff + ".Description", demonSinGuzzlerBuffDescription);

            demonSinGuzzlerBuff.EditComponent<Polymorph>(c => {
                c.m_Prefab = sW_GlabrezuBoss.Prefab;
                c.NaturalArmor = 8;
                c.m_Facts = new BlueprintUnitFactReference[]{
                        BlueprintTool.Get<BlueprintAbility>("bd09b025ee2a82f46afab922c4decca9").ToReference<BlueprintUnitFactReference>(),
                        BlueprintTool.Get<BlueprintFeature>("b555e9c8da67a7344ae0bba48b706f53").ToReference<BlueprintUnitFactReference>(),
                        BlueprintTool.Get<BlueprintFeature>("28549cff02334f2cbf6724080944ce42").ToReference<BlueprintUnitFactReference>(),
                        BlueprintTool.Get<BlueprintFeature>("70cffb448c132fa409e49156d013b175").ToReference<BlueprintUnitFactReference>(),
                        BlueprintTool.Get<BlueprintFeature>("e19200a99d215074c935cb04175c5665").ToReference<BlueprintUnitFactReference>()
            };
                c.StrengthBonus = 10;
                c.ConstitutionBonus = 6;
            });

            demonSinGuzzlerBuff.EditComponent<ReplaceAsksList>(c => {
                c.m_Asks = sW_GlabrezuBoss.Visual.m_Barks;
            });

            Helpers.AddBlueprint(demonSinGuzzlerBuff, demonSinGuzzlerBuffGuid);

            Main.Log("Demon Sin Guzzler Form Buff Added" + demonSinGuzzlerBuff.AssetGuid.ToString());
            ///
            var demonSinGuzzlerFormGuid = new BlueprintGuid(new Guid("d5139a7d-4a57-41b3-afa6-bea8baa7fe29"));

            var demonSinGuzzlerForm = Helpers.CreateCopy(DemonicFormIIIGlabrezu, bp =>
            {
                bp.AssetGuid = demonSinGuzzlerFormGuid;
                bp.name = "Demonic Sin Guzzler Balor Transformation"+ bp.AssetGuid;
                bp.m_DisplayName = demonSinGuzzlerBuff.m_DisplayName;
                bp.m_Icon = demonSinGuzzlerBuff.m_Icon;
                bp.m_Description = demonSinGuzzlerBuff.m_Description;
            });

            demonSinGuzzlerForm.RemoveComponents<ContextRankConfig>();

            var demonSinGuzzlerFormContextApplyAction = Helpers.Create<ContextActionApplyBuff>(c => {
                c.ToCaster = true;
                c.m_Buff = demonSinGuzzlerBuff.ToReference<BlueprintBuffReference>();
                c.UseDurationSeconds = true;
                c.DurationSeconds = 86400;
            });

            demonSinGuzzlerForm.GetComponent<AbilityEffectRunAction>().Actions.Actions[1] = demonSinGuzzlerFormContextApplyAction;

            Helpers.AddBlueprint(demonSinGuzzlerForm, demonSinGuzzlerFormGuid);
            ///

            var demonSinGuzzlerPolymorphFeatureGuid = new BlueprintGuid(new Guid("6c8f47ae-288f-44d5-bfa6-21d2c91b7594"));

            var demonSinGuzzlerPolymorphFeature = Helpers.Create<BlueprintFeature>(c =>
            {
                c.AssetGuid = demonSinGuzzlerPolymorphFeatureGuid;
                c.name = "Unleashed Sin Guzzler" + c.AssetGuid;
                c.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonPolymorph.png");
                c.Ranks = 1;
                c.IsClassFeature = true;
                c.ReapplyOnLevelUp = true;
                c.HideInCharacterSheetAndLevelUp = true;
                c.m_DescriptionShort = new LocalizedString();
                c.Groups = new FeatureGroup[] { };
                c.HideInUI = true;
                c.HideNotAvailibleInUI = true;
            });
            demonSinGuzzlerPolymorphFeature.m_DisplayName = Helpers.CreateString(demonSinGuzzlerPolymorphFeature + ".Name", "Unleashed Sin Guzzler");
            var demonSinGuzzlerPolymorphDescription = "You may transform into a Sin Guzzler at-will for 24 hours.";
            demonSinGuzzlerPolymorphFeature.m_Description = Helpers.CreateString(demonSinGuzzlerPolymorphFeature + ".Description", demonSinGuzzlerPolymorphDescription);

            demonSinGuzzlerPolymorphFeature.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[]{
                        demonSinGuzzlerForm.ToReference<BlueprintUnitFactReference>()
                    };
            });


            Helpers.AddBlueprint(demonSinGuzzlerPolymorphFeature, demonSinGuzzlerPolymorphFeatureGuid);

            Main.Log("Demon SinGuzzler Added" + demonSinGuzzlerPolymorphFeature.AssetGuid.ToString());

            ///

            var demonPolymorphFeatureGuid = new BlueprintGuid(new Guid("bbd26df5-1304-4c01-a56b-5fad024a86bc"));

            var demonPolymorphFeature = Helpers.Create<BlueprintFeature>(c =>
            {
                c.AssetGuid = demonPolymorphFeatureGuid;
                c.name = "Unleashed Demon"+c.AssetGuid;
                c.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonPolymorph.png");
                c.Ranks = 1;
                c.IsClassFeature = true;
                c.ReapplyOnLevelUp = true;
                c.HideInCharacterSheetAndLevelUp = true;
                c.m_DescriptionShort = new LocalizedString();
                c.Groups = new FeatureGroup[] { };
            });
            demonPolymorphFeature.m_DisplayName = Helpers.CreateString(demonPolymorphFeature + ".Name", "Unleashed Demon");
            var demonPolymorphDescription = "You learn you unleash the demons within you.\n" +
                                            "While polymorphed into a demonic form, for each attack that hits you deal an extra 1d6 + Mythic Rank extra Unholy damage and have a 15% chance to restore a round of Demon Rage.\n" +
                                            "You also gain the ability to transform yourself into a Balor as Demon Form IV for 1 minute. You may do this an addtional time at 6th and 9th mythic rank.\n" +
                                            "At 9th mythic rank, you gain the ability to tranform into a Sin Guzzler, a powerful version of a Glabrezu, at will.";
            demonPolymorphFeature.m_Description = Helpers.CreateString(demonPolymorphFeature + ".Description", demonPolymorphDescription);

            demonPolymorphFeature.AddComponent<AddAbilityResources>(c =>
            {
                c.RestoreAmount = true;
                c.m_Resource = demonPolyResource.ToReference<BlueprintAbilityResourceReference>();
            });

            var contextResourceIncrease = Helpers.Create<ContextActionIncreaseRageRounds>(c =>
            {
                c.m_resource = demonRageResource;
                c.m_resourceAmount = 1;
                c.m_RandomChance = true;
                c.m_RandomChancePercent = 15;
            });

            var polymorphCondition = new Condition[] {
                            new ContextConditionCasterHasBuff
                            {
                                m_Buffs = new BlueprintBuff[]{
                                        demonicFormIBabauBuff,
                                        demonicFormIVGalluBuff,
                                        demonicFormIBabauBuff,
                                        demonicFormIBrimorakBuff,
                                        demonicFormIIIVrockBuff,
                                        demonicFormIIDerakniBuff,
                                        demonicFormIVVavakiaBuff,
                                        demonicFormIVBalorBuff,
                                        demonicFormIVMarilithBuff,
                                        demonicFormIIKalavakusBuff,
                                        demonicFormIIINalfeshneeBuff,
                                        demonicFormIINabasuBuff,
                                        demonicFormIIIGlabrezuBuff,
                                        demonSinGuzzlerBuff
                                }
                            }
                };

            var polymorphConditionsChecker = new ConditionsChecker()
            {
                Conditions = polymorphCondition,
                Operation = Operation.Or
            };


            demonPolymorphFeature.AddComponent<ContextRankConfig>(c =>
            {
                c.m_Type = AbilityRankType.DamageBonus;
                c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                c.m_Progression = ContextRankProgression.AsIs;
            });


            var demonPolyDamageUnholy = Helpers.Create<ContextActionDealDamage>(c =>
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
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageBonus
                    }
                };
            });

            var conditionalAttack = new Conditional()
            {
                ConditionsChecker = polymorphConditionsChecker,
                IfTrue = new ActionList()
                {
                    Actions = new GameAction[] { contextResourceIncrease, demonPolyDamageUnholy }
                },
                IfFalse = new ActionList()
            };

            demonPolymorphFeature.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
            {
                c.OnlyHit = true;
                c.Action = new ActionList();
                c.Action.Actions = new GameAction[] { conditionalAttack };
            });


            demonPolymorphFeature.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[]{
                        demonBalorForm.ToReference<BlueprintUnitFactReference>()
                    };
            });


            Helpers.AddBlueprint(demonPolymorphFeature, demonPolymorphFeatureGuid);

            Main.Log("Demon Polymorph Added" + demonPolymorphFeature.AssetGuid.ToString());

        }

    }
}
