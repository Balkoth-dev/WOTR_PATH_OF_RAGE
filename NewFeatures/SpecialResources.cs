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
            var demonMythicClass = BlueprintTool.Get<BlueprintCharacterClass>("8e19495ea576a8641964102d177e34b7").ToReference<BlueprintCharacterClassReference>();
            var demonSmashResourceGuid = new BlueprintGuid(new Guid("40536705-671e-4e96-979a-10a41ea6057e"));

            var demonSmashResource = Helpers.Create<BlueprintAbilityResource>(bp => 
            {
                bp.AssetGuid = demonSmashResourceGuid;
                bp.name = "DemonSmashResource"+ bp.AssetGuid;
                bp.m_MaxAmount.m_ClassDiv = new BlueprintCharacterClassReference[] { demonMythicClass };
                bp.m_MaxAmount.BaseValue = 2;
                bp.m_MaxAmount.StartingLevel = 2;
                bp.m_MaxAmount.LevelStep = 1;
                bp.m_MaxAmount.PerStepIncrease = 1;
                bp.m_MaxAmount.IncreasedByLevel = true;
                bp.m_MaxAmount.IncreasedByLevelStartPlusDivStep = true;
                bp.Components = new BlueprintComponent[]{ };
            });

            Helpers.AddBlueprint(demonSmashResource, demonSmashResourceGuid);

            var demonSoulResourceGuid = new BlueprintGuid(new Guid("0c9c6e72-90a9-412f-ade8-d97291a005e3"));

            var demonSoulResource = Helpers.Create<BlueprintAbilityResource>(bp =>
            {
                bp.AssetGuid = demonSoulResourceGuid;
                bp.name = "demonSoulResource" + bp.AssetGuid;
                bp.m_MaxAmount.m_ClassDiv = new BlueprintCharacterClassReference[] { demonMythicClass };
                bp.m_MaxAmount.BaseValue = 1;
                bp.m_MaxAmount.StartingLevel = 3;
                bp.m_MaxAmount.LevelStep = 3;
                bp.m_MaxAmount.PerStepIncrease = 1;
                bp.m_MaxAmount.IncreasedByLevel = true;
                bp.m_MaxAmount.IncreasedByLevelStartPlusDivStep = true;
                bp.Components = new BlueprintComponent[] { };
            });

            Helpers.AddBlueprint(demonSoulResource, demonSoulResourceGuid);

            var demonPolyResourceGuid = new BlueprintGuid(new Guid("fb938b3d-9deb-46b3-b3a4-4de61cd2d574"));

            var demonPolyResource = Helpers.Create<BlueprintAbilityResource>(bp =>
            {
                bp.AssetGuid = demonPolyResourceGuid;
                bp.name = "demonPolyResource" + bp.AssetGuid;
                bp.m_MaxAmount.m_ClassDiv = new BlueprintCharacterClassReference[] { demonMythicClass };
                bp.m_MaxAmount.BaseValue = 1;
                bp.m_MaxAmount.StartingLevel = 3;
                bp.m_MaxAmount.LevelStep = 3;
                bp.m_MaxAmount.PerStepIncrease = 1;
                bp.m_MaxAmount.IncreasedByLevel = true;
                bp.m_MaxAmount.IncreasedByLevelStartPlusDivStep = true;
                bp.Components = new BlueprintComponent[] { };
            });

            Helpers.AddBlueprint(demonPolyResource, demonPolyResourceGuid);

            var demonRipResourceGuid = new BlueprintGuid(new Guid("32444033-a717-4b6a-8c10-e2cff8810233"));

            var demonRipResource = Helpers.Create<BlueprintAbilityResource>(bp =>
            {
                bp.AssetGuid = demonRipResourceGuid;
                bp.name = "demonRipResource" + bp.AssetGuid;
                bp.m_MaxAmount.m_ClassDiv = new BlueprintCharacterClassReference[] { demonMythicClass };
                bp.m_MaxAmount.BaseValue = 3;
                bp.m_MaxAmount.StartingLevel = 1;
                bp.m_MaxAmount.LevelStep = 1;
                bp.m_MaxAmount.PerStepIncrease = 1;
                bp.m_MaxAmount.IncreasedByLevel = true;
                bp.m_MaxAmount.IncreasedByLevelStartPlusDivStep = true;
                bp.Components = new BlueprintComponent[] { };
            });

            Helpers.AddBlueprint(demonRipResource, demonRipResourceGuid);

        }

    }
}
