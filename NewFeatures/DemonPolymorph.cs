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
using Kingmaker.Blueprints.Items.Ecnchantments;

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

            BlueprintAbility demonicFormIVBalor = BlueprintTool.Get<BlueprintAbility>("0258a875670fa134590c6ffdc23da2cf");
           
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
            var demonPolymorphFeatureGuid = new BlueprintGuid(new Guid("bbd26df5-1304-4c01-a56b-5fad024a86bc"));

            var demonPolymorphFeature = Helpers.Create<BlueprintFeature>(c =>
            {
                c.AssetGuid = demonPolymorphFeatureGuid;
                c.name = "Unleashed Demon";
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
                                            "In addition, you gain the ability to transform yourself into a Balor as Demon Form IV for 1 minute. You may do this an addtional time at 6th and 9th mythic rank.";
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
                                        demonicFormIIIGlabrezuBuff
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
