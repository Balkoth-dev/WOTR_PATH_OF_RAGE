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
    class SpecialResources
    {
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

            var demonPolyResourceGuid = new BlueprintGuid(new Guid("fb938b3d-9deb-46b3-b3a4-4de61cd2d574"));

            var demonPolyResource = Helpers.CreateCopy(demonRageResource, bp =>
            {
                bp.AssetGuid = demonSoulResourceGuid;
                bp.m_MaxAmount.BaseValue = 1;
                bp.m_MaxAmount.StartingLevel = 3;
                bp.m_MaxAmount.LevelStep = 3;
                bp.m_MaxAmount.PerStepIncrease = 1;
            });

            Helpers.AddBlueprint(demonPolyResource, demonPolyResourceGuid);

        }

    }
}
