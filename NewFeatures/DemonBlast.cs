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
    class DemonBlast
    {
        public static void AddDemonBlast()
        {

            var demonChargeMainAbility = BlueprintTool.Get<BlueprintAbility>("1b677ed598d47a048a0f6b4b671b8f84");

            var demonBlastGuid = new BlueprintGuid(new Guid("6fc3b519-1853-4144-9e4a-4fd803b34d35"));

            var demonBlast = Helpers.CreateCopy(demonChargeMainAbility, bp =>
            {
                bp.AssetGuid = demonBlastGuid;
                bp.m_Icon = AssetLoader.LoadInternal("Abilities", "DemonBlast.png");
                bp.Range = AbilityRange.Personal;
                bp.CanTargetSelf = true;
                bp.name = "Demon Blast";

            });
            demonBlast.m_DisplayName = Helpers.CreateString(demonBlast + ".Name", "Demonic Blast");
            demonBlast.LocalizedSavingThrow = Helpers.CreateString(demonBlast + ".SavingThrow", "None");
            var demonSoulDescription = "As a {g|Encyclopedia:Move_Action}move action{/g}, you can let loose an explosion, dealing {g|Encyclopedia:Dice}2d6{/g} unholy {g|Encyclopedia:Damage}damage{/g} " +
                "per mythic rank to all enemies in a 10 feet range.\n An enemy can only be damaged by this ability or Demonic Charge once per {g|Encyclopedia:Combat_Round}round{/g}.";
            demonBlast.m_Description = Helpers.CreateString(demonBlast + ".Description", demonSoulDescription);

            demonBlast.RemoveComponents<AbilityCustomTeleportation>();

            Helpers.AddBlueprint(demonBlast, demonBlastGuid);

            Main.Log("Demon Blast Added");

            var demonBlastFeatureGuid = new BlueprintGuid(new Guid("6cf0d55c-050c-497a-8b98-e245435ce6aa"));

            var demonBlastFeature = Helpers.Create<BlueprintFeature>(c =>
            {
                c.AssetGuid = demonBlastFeatureGuid;
                c.m_DisplayName = demonBlast.m_DisplayName;
                c.m_Description = demonBlast.m_Description;
                c.m_Icon = demonBlast.m_Icon;
                c.Ranks = 1;
                c.IsClassFeature = true;
                c.ReapplyOnLevelUp = true;
                c.HideInCharacterSheetAndLevelUp = true;
                c.m_DescriptionShort = new LocalizedString();
                c.Groups = new FeatureGroup[] { };
                c.name = "Demon Blast";
            });

            demonBlastFeature.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[]{
                        demonBlast.ToReference<BlueprintUnitFactReference>()
                    };
            });

            Helpers.AddBlueprint(demonBlastFeature, demonBlastFeatureGuid);

            Main.Log("Demon Blast Feature Created" + demonBlastFeatureGuid);

            if (Main.settings.AddDemonBlast == false)
            {
                return;
            }
            var demonProgression = BlueprintTool.Get<BlueprintProgression>("285fe49f7df8587468f676aa49362213");

            demonProgression.LevelEntries[0].m_Features.Add(demonBlastFeature.ToReference<BlueprintFeatureBaseReference>());

            Main.Log("Demon Blast Added To Mythic");

        }

    }
}
