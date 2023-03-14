using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using WOTR_PATH_OF_RAGE.Utilities;
using Kingmaker.Blueprints;
using WOTR_PATH_OF_RAGE;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Localization;
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
using WOTR_PATH_OF_RAGE.New_Rules;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using WOTR_PATH_OF_RAGE.MechanicsChanges;
using Kingmaker.Blueprints.Items.Ecnchantments;
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Buffs;
using System.Collections.Generic;
using System.Linq;

namespace WOTR_PATH_OF_RAGE.NewFeatures
{
    class DemonPolymorph
    {
        public static void AddDemonPolymorph()
        {
            BlueprintAbilityResourceReference demonRageResource = BlueprintTool.Get<BlueprintAbilityResource>("bc2c2f64ada54c78a250f8b72c48ae57").ToReference<BlueprintAbilityResourceReference>();

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

            var shifterWildShapeFeyBuff = BlueprintTool.Get<BlueprintBuff>("477259fe81a647ad9a38b47140e38de6");
            var shifterWildShapeBearBuff = BlueprintTool.Get<BlueprintBuff>("68ca4094f4e7488c8e869af833e153f1");
            var shifterWildShapeFeyBuff8 = BlueprintTool.Get<BlueprintBuff>("6fc7df0ddb9a466d976a3808a8f1437a");
            var shifterWildShapeBoarBuff = BlueprintTool.Get<BlueprintBuff>("c6b493bf01ec4478bbb7247f56d670f8");
            var shifterWildShapeWolfBuff = BlueprintTool.Get<BlueprintBuff>("fe61e45eb3d1441795179eb0bff1ef3b");
            var shifterWildShapeFeyBuff15 = BlueprintTool.Get<BlueprintBuff>("dee544177d0148edbaa7ca6a0aee03c0");
            var shifterWildShapeTigerBuff = BlueprintTool.Get<BlueprintBuff>("7a5da4b80a494bf2b96fa7756d3f89cc");
            var shifterWildShapeHorseBuff = BlueprintTool.Get<BlueprintBuff>("b3e5540f487d4a2b9aa50aad34c60ec3");
            var shifterWildShapeBoarBuff8 = BlueprintTool.Get<BlueprintBuff>("ae71fed93f9f442cbce5b601e4aa1a23");
            var shifterWildShapeWolfBuff8 = BlueprintTool.Get<BlueprintBuff>("a983157daa464ab7a725ed5f53110a32");
            var shifterWildShapeBearBuff8 = BlueprintTool.Get<BlueprintBuff>("9cc0eceb9cbc44e4b1ecc5e8b5f97c45");
            var shifterWildShapeWolfBuff15 = BlueprintTool.Get<BlueprintBuff>("14bf6a8c9f9f40dba2f6fc41e6235270");
            var shifterWildShapeBoarBuff15 = BlueprintTool.Get<BlueprintBuff>("b8c0ad632442496aa20b96864dde4454");
            var shifterWildShapeTigerBuff8 = BlueprintTool.Get<BlueprintBuff>("defa946808514ab0a1023192fa13ede3");
            var shifterWildShapeSpiderBuff = BlueprintTool.Get<BlueprintBuff>("58e7d688873242f38b6cc66a7ae3d794");
            var shifterWildShapeBearBuff15 = BlueprintTool.Get<BlueprintBuff>("5adecdc82eda4271b6dc4a7e6a921c89");
            var shifterWildShapeHorseBuff8 = BlueprintTool.Get<BlueprintBuff>("4a5e4c66ca8d4657b281060a24886ad1");
            var shifterWildShapeLizardBuff = BlueprintTool.Get<BlueprintBuff>("0b15a8dce67746e1979dfc597f13827f");
            var shifterWildShapeTigerBuff15 = BlueprintTool.Get<BlueprintBuff>("f49109fdb0a24f75a4ab466bf95843af");
            var shifterWildShapeHorseBuff15 = BlueprintTool.Get<BlueprintBuff>("f493bc3e006d44fb8815267ffa49ec76");
            var shifterWildShapeGriffonBuff = BlueprintTool.Get<BlueprintBuff>("e76d475eb1f1470e9950a5fee99ddb40");
            var shifterWildShapeLizardBuff8 = BlueprintTool.Get<BlueprintBuff>("f246e4e203f84f52bed2dc16e6d36087");
            var shifterWildShapeSpiderBuff8 = BlueprintTool.Get<BlueprintBuff>("c163b127ce354363a20946bb01967cac");
            var shifterWildShapeDinosaurBuff = BlueprintTool.Get<BlueprintBuff>("bdbc24300ebd4fd6810172e4d4b1ab19");
            var shifterWildShapeGriffonBuff9 = BlueprintTool.Get<BlueprintBuff>("3a7511f1b8a94b11bbb21245e150c0b6");
            var shifterWildShapeElephantBuff = BlueprintTool.Get<BlueprintBuff>("7e90cc7393624b13b6d319774bf6d812");
            var shifterWildShapeLizardBuff15 = BlueprintTool.Get<BlueprintBuff>("ba5b0df349c94c91ad7a38309b042537");
            var shifterWildShapeSpiderBuff15 = BlueprintTool.Get<BlueprintBuff>("d895c3b8663f4b73a3868e6369bba6d4");
            var shifterWildShapeManticoreBuff = BlueprintTool.Get<BlueprintBuff>("91f12a442b374bd7bfdfb05f5ab80f4c");
            var shifterWildShapeDinosaurBuff8 = BlueprintTool.Get<BlueprintBuff>("f6d0e7d4d96b4f7897557968a0706f74");
            var shifterWildShapeElephantBuff8 = BlueprintTool.Get<BlueprintBuff>("38f84f5a0da94b169131b863be81957b");
            var shifterWildShapeGriffonBuff14 = BlueprintTool.Get<BlueprintBuff>("821fa7f586ca44238a0894115824035c");
            var shifterWildShapeWolverineBuff = BlueprintTool.Get<BlueprintBuff>("350ba136f5d04e588f1a6e3ba22233cb");
            var shifterWildShapeElephantBuff15 = BlueprintTool.Get<BlueprintBuff>("fdb39ec525c2411aad43f6b12ad4b1c0");
            var shifterWildShapeGriffonGodBuff = BlueprintTool.Get<BlueprintBuff>("4b95ed9a351e4effbb2a83e246ee6334");
            var shifterWildShapeDinosaurBuff15 = BlueprintTool.Get<BlueprintBuff>("ab0f757d55a74658b812829518566df4");
            var shifterWildShapeManticoreBuff8 = BlueprintTool.Get<BlueprintBuff>("28861899db294aa593ada213a8d1fd36");
            var shifterWildShapeWolverineBuff8 = BlueprintTool.Get<BlueprintBuff>("12272456258f4334a11b87610cf33abd");
            var shifterWildShapeWolverineBuff15 = BlueprintTool.Get<BlueprintBuff>("cd5f29d5f50a43f9a2d5f3debce86477");
            var shifterWildShapeGriffonGodBuff9 = BlueprintTool.Get<BlueprintBuff>("d8b979bf19554b85bbed05e6369c0f63");
            var shifterWildShapeManticoreBuff15 = BlueprintTool.Get<BlueprintBuff>("f5331d1b15ac4c4a833e2928ce3bf18d");
            var shifterWildShapeGriffonGodBuff14 = BlueprintTool.Get<BlueprintBuff>("10c913c645364bafbde759f83d103ce6");
            var shifterWildShapeGriffonDemonBuff = BlueprintTool.Get<BlueprintBuff>("6236b745b60a4a578c435c861df393f4");
            var shifterWildShapeGriffonDemonBuff9 = BlueprintTool.Get<BlueprintBuff>("7d4798e5fe5f4a349b56686340008824");
            var shifterWildShapeGriffonDemonBuff14 = BlueprintTool.Get<BlueprintBuff>("431ca9188d6f401f9f8df8079c526e59");

            var keen = BlueprintTool.Get<BlueprintWeaponEnchantment>("102a9c8c9b7a75e4fb5844e79deaf4c0").ToReference<BlueprintFeatureReference>();

            var demonPolyResource = BlueprintTool.Get<BlueprintAbilityResource>("fb938b3d9deb46b3b3a44de61cd2d574");

            var sW_GlabrezuBoss = BlueprintTool.Get<BlueprintUnit>("2433f902e6d443e2a6010de9e4227e9e");

            var bite2d6 = BlueprintTool.Get<BlueprintItemWeapon>("2abc1dc6172759c42971bd04b8c115cb").ToReference<BlueprintItemWeaponReference>();
            var claw1d6 = BlueprintTool.Get<BlueprintItemWeapon>("65eb73689b94d894080d33a768cdf645").ToReference<BlueprintItemWeaponReference>();
            var sting1d4 = BlueprintTool.Get<BlueprintItemWeapon>("df44800dbe7b4ba43ac6e0e435041ed8").ToReference<BlueprintItemWeaponReference>();

            ///
            var demonicFormIVBalor = BlueprintTool.Get<BlueprintAbility>("0258a875670fa134590c6ffdc23da2cf");
            var DemonicFormIIIGlabrezu = BlueprintTool.Get<BlueprintAbility>("0258a875670fa134590c6ffdc23da2cf");


            var unstableDemonicFormIVBalorBuffGuild = new BlueprintGuid(new Guid("70c2561e-9fa9-4633-930c-815b03f90f4f"));

            var unstableDemonicFormIVBalorBuff = Helpers.CreateCopy(demonicFormIVBalorBuff, bp =>
            {
                bp.AssetGuid = unstableDemonicFormIVBalorBuffGuild;
                bp.name = "Unstable Balor Transformation";
            });

            unstableDemonicFormIVBalorBuff.AddComponent<AddCondition>(c =>
            {
                c.Condition = Kingmaker.UnitLogic.UnitCondition.AttackNearest;
            });

            Helpers.AddBlueprint(unstableDemonicFormIVBalorBuff, unstableDemonicFormIVBalorBuffGuild);

            var unstableDemonBalorFormGuid = new BlueprintGuid(new Guid("e30768ff-138f-4ab5-b947-03df2aa72a6a"));

            var unstableDemonBalorForm = Helpers.CreateCopy(demonicFormIVBalor, bp =>
            {
                bp.AssetGuid = unstableDemonBalorFormGuid;
                bp.name = "Unstable Balor Transformation";
            });

            unstableDemonBalorForm.RemoveComponents<ContextRankConfig>();

            var demonBalorFormContextActionApplyBuff = (ContextActionApplyBuff)unstableDemonBalorForm.GetComponent<AbilityEffectRunAction>().Actions.Actions[1];
            demonBalorFormContextActionApplyBuff.DurationValue = null;
            demonBalorFormContextActionApplyBuff.DurationSeconds = 30;
            demonBalorFormContextActionApplyBuff.UseDurationSeconds = true;

            unstableDemonBalorForm.AddComponent<AbilityResourceLogic>(c =>
            {
                c.Amount = 1;
                c.m_IsSpendResource = true;
                c.m_RequiredResource = demonPolyResource.ToReference<BlueprintAbilityResourceReference>();
            });

            var demonBalorFormAbilityEffectRunAction = unstableDemonBalorForm.GetComponent<AbilityEffectRunAction>();

            foreach (var v in demonBalorFormAbilityEffectRunAction.Actions.Actions.OfType<ContextActionApplyBuff>())
            {
                v.m_Buff = unstableDemonicFormIVBalorBuff.ToReference<BlueprintBuffReference>();
            }

            unstableDemonBalorForm.m_DisplayName = Helpers.CreateString(unstableDemonBalorForm + ".Name", "Unstable Balor Form");

            Helpers.AddBlueprint(unstableDemonBalorForm, unstableDemonBalorFormGuid);

            Main.Log("Demon Balor Form Added" + unstableDemonBalorForm.AssetGuid.ToString());
            ///

            var demonSinGuzzlerBuffGuid = new BlueprintGuid(new Guid("e0edd270-bb50-4b45-937f-b60d047e0fd5"));

            var demonSinGuzzlerBuff = Helpers.CreateCopy(demonicFormIIIGlabrezuBuff, bp =>
            {
                bp.AssetGuid = demonSinGuzzlerBuffGuid;
                bp.name = "Demonic Form Sin Guzzler" + bp.AssetGuid;
                bp.m_Icon = DemonicFormIIIGlabrezu.m_Icon;
            });
            demonSinGuzzlerBuff.m_DisplayName = Helpers.CreateString(demonSinGuzzlerBuff + ".Name", "Demonic Form - Sin Guzzler");
            var demonSinGuzzlerBuffDescription = "You transform into a Sin Guzzler. Gaining the Rend of the Glabrezu, DR10/Good, and are considered airborne.";
            demonSinGuzzlerBuff.m_Description = Helpers.CreateString(demonSinGuzzlerBuff + ".Description", demonSinGuzzlerBuffDescription);

            demonSinGuzzlerBuff.EditComponent<Polymorph>(c =>
            {
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

            demonSinGuzzlerBuff.EditComponent<ReplaceAsksList>(c =>
            {
                c.m_Asks = sW_GlabrezuBoss.Visual.m_Barks;
            });

            Helpers.AddBlueprint(demonSinGuzzlerBuff, demonSinGuzzlerBuffGuid);

            Main.Log("Demon Sin Guzzler Form Buff Added" + demonSinGuzzlerBuff.AssetGuid.ToString());
            ///
            var demonSinGuzzlerFormGuid = new BlueprintGuid(new Guid("d5139a7d-4a57-41b3-afa6-bea8baa7fe29"));

            var demonSinGuzzlerForm = Helpers.CreateCopy(DemonicFormIIIGlabrezu, bp =>
            {
                bp.AssetGuid = demonSinGuzzlerFormGuid;
                bp.name = "Demonic Sin Guzzler Balor Transformation" + bp.AssetGuid;
                bp.m_DisplayName = demonSinGuzzlerBuff.m_DisplayName;
                bp.m_Icon = demonSinGuzzlerBuff.m_Icon;
                bp.m_Description = demonSinGuzzlerBuff.m_Description;
            });

            demonSinGuzzlerForm.RemoveComponents<ContextRankConfig>();

            var demonSinGuzzlerFormContextApplyAction = Helpers.Create<ContextActionApplyBuff>(c =>
            {
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
                c.name = "Unleashed Demon" + c.AssetGuid;
                c.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonPolymorph.png");
                c.Ranks = 1;
                c.IsClassFeature = true;
                c.ReapplyOnLevelUp = true;
                c.HideInCharacterSheetAndLevelUp = true;
                c.m_DescriptionShort = new LocalizedString();
                c.Groups = new FeatureGroup[] { };
                c.IsPrerequisiteFor = new List<BlueprintFeatureReference>();
                c.IsPrerequisiteFor.Add(demonSinGuzzlerPolymorphFeature.ToReference<BlueprintFeatureReference>());
            });
            demonPolymorphFeature.m_DisplayName = Helpers.CreateString(demonPolymorphFeature + ".Name", "Unleashed Demon");
            var demonPolymorphDescription = "You learn to unleash the demons within you.\n" +
                                            "While polymorphed into a demonic or shifter form for each attack that hits, you deal an extra 1d6 + Mythic Rank extra Unholy damage and have a 15% chance to restore a round of Demon Rage.\n" +
                                            "You also gain the ability to transform yourself into an unstable Balor as Demon Form IV for 30 seconds, attacking anything and anyone near you. You may do this an addtional every mythic rank.\n" +
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
                                        demonSinGuzzlerBuff,
                                        unstableDemonicFormIVBalorBuff,
                                        shifterWildShapeFeyBuff,
                                        shifterWildShapeBearBuff,
                                        shifterWildShapeFeyBuff8,
                                        shifterWildShapeBoarBuff,
                                        shifterWildShapeWolfBuff,
                                        shifterWildShapeFeyBuff15,
                                        shifterWildShapeTigerBuff,
                                        shifterWildShapeHorseBuff,
                                        shifterWildShapeBoarBuff8,
                                        shifterWildShapeWolfBuff8,
                                        shifterWildShapeBearBuff8,
                                        shifterWildShapeWolfBuff15,
                                        shifterWildShapeBoarBuff15,
                                        shifterWildShapeTigerBuff8,
                                        shifterWildShapeSpiderBuff,
                                        shifterWildShapeBearBuff15,
                                        shifterWildShapeHorseBuff8,
                                        shifterWildShapeLizardBuff,
                                        shifterWildShapeTigerBuff15,
                                        shifterWildShapeHorseBuff15,
                                        shifterWildShapeGriffonBuff,
                                        shifterWildShapeLizardBuff8,
                                        shifterWildShapeSpiderBuff8,
                                        shifterWildShapeDinosaurBuff,
                                        shifterWildShapeGriffonBuff9,
                                        shifterWildShapeElephantBuff,
                                        shifterWildShapeLizardBuff15,
                                        shifterWildShapeSpiderBuff15,
                                        shifterWildShapeManticoreBuff,
                                        shifterWildShapeDinosaurBuff8,
                                        shifterWildShapeElephantBuff8,
                                        shifterWildShapeGriffonBuff14,
                                        shifterWildShapeWolverineBuff,
                                        shifterWildShapeElephantBuff15,
                                        shifterWildShapeGriffonGodBuff,
                                        shifterWildShapeDinosaurBuff15,
                                        shifterWildShapeManticoreBuff8,
                                        shifterWildShapeWolverineBuff8,
                                        shifterWildShapeWolverineBuff15,
                                        shifterWildShapeGriffonGodBuff9,
                                        shifterWildShapeManticoreBuff15,
                                        shifterWildShapeGriffonGodBuff14,
                                        shifterWildShapeGriffonDemonBuff,
                                        shifterWildShapeGriffonDemonBuff9,
                                        shifterWildShapeGriffonDemonBuff14
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
                        unstableDemonBalorForm.ToReference<BlueprintUnitFactReference>()
                    };
            });

            demonSinGuzzlerPolymorphFeature.AddComponent<PrerequisiteFeature>(c =>
            {
                c.m_Feature = demonPolymorphFeature.ToReference<BlueprintFeatureReference>();
            });

            demonSinGuzzlerForm.AddComponent<PrerequisiteFeature>(c =>
            {
                c.m_Feature = demonPolymorphFeature.ToReference<BlueprintFeatureReference>();
            });

            demonSinGuzzlerForm.AddComponent<AbilityCasterHasFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[]{
                        demonPolymorphFeature.ToReference<BlueprintUnitFactReference>()
                    };
            });

            demonSinGuzzlerForm.AddComponent<AbilityShowIfCasterHasFact>(c =>
            {
                c.m_UnitFact = demonPolymorphFeature.ToReference<BlueprintUnitFactReference>();
            });

            Helpers.AddBlueprint(demonPolymorphFeature, demonPolymorphFeatureGuid);

            Main.Log("Demon Polymorph Added" + demonPolymorphFeature.AssetGuid.ToString());

        }

    }
}
