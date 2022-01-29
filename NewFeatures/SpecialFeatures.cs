
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
using BlueprintCore.Utils;
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
    class SpecialFeatures
    {
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
    }
}
