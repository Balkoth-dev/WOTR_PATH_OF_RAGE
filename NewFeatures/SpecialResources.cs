
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
using BlueprintCore.Utils;

namespace WOTR_PATH_OF_RAGE.NewFeatures
{
    class SpecialResources
    {
        public static void AddDemonSpecialResources()
        {
            var demonMythicClass = BlueprintTool.Get<BlueprintCharacterClass>("8e19495ea576a8641964102d177e34b7").ToReference<BlueprintCharacterClassReference>();
            var demonSmashResourceGuid = new BlueprintGuid(new Guid("40536705-671e-4e96-979a-10a41ea6057e"));

            var demonRageResource = BlueprintTool.Get<BlueprintAbilityResource>("f3bf174f0f86b4f45a823e9ed6ccc7a5");

            var newDemonRageResourceGuid = new BlueprintGuid(new Guid("bc2c2f64-ada5-4c78-a250-f8b72c48ae57"));

            var newDemonRageResource = Helpers.CreateCopy(demonRageResource, bp =>
            {
                bp.AssetGuid = newDemonRageResourceGuid;
                bp.name = "New Demon Rage Resource" + bp.AssetGuid;
            });

            newDemonRageResource.m_MaxAmount.BaseValue = 11;
            newDemonRageResource.m_MaxAmount.StartingLevel = 1;
            newDemonRageResource.m_MaxAmount.LevelStep = 1;
            newDemonRageResource.m_MaxAmount.PerStepIncrease = 3;

            Helpers.AddBlueprint(newDemonRageResource, newDemonRageResourceGuid);

            var demonSmashResource = Helpers.Create<BlueprintAbilityResource>(bp => 
            {
                bp.AssetGuid = demonSmashResourceGuid;
                bp.name = "DemonSmashResource"+ bp.AssetGuid;
                bp.m_Min = 1;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount
                {
                    BaseValue = 1,
                    IncreasedByStat = false,
                    m_ClassDiv = new BlueprintCharacterClassReference[] { demonMythicClass },
                    m_Class = new BlueprintCharacterClassReference[] { demonMythicClass },
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                    IncreasedByLevelStartPlusDivStep = true,
                    StartingLevel = 1,
                    LevelStep = 2,
                    PerStepIncrease = 1,
                    StartingIncrease = 1
                };
            });

            Helpers.AddBlueprint(demonSmashResource, demonSmashResourceGuid);

            var demonSoulResourceGuid = new BlueprintGuid(new Guid("0c9c6e72-90a9-412f-ade8-d97291a005e3"));

            var demonSoulResource = Helpers.Create<BlueprintAbilityResource>(bp =>
            {
                bp.AssetGuid = demonSoulResourceGuid;
                bp.name = "DemonSoulResource" + bp.AssetGuid;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount
                {
                    BaseValue = 0,
                    IncreasedByStat = false,
                    m_ClassDiv = new BlueprintCharacterClassReference[] { demonMythicClass },
                    m_Class = new BlueprintCharacterClassReference[] { demonMythicClass },
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                    IncreasedByLevelStartPlusDivStep = true,
                    StartingLevel = 1,
                    LevelStep = 3,
                    PerStepIncrease = 1,
                    StartingIncrease = 1
                };
            });

            Helpers.AddBlueprint(demonSoulResource, demonSoulResourceGuid);

            var demonPolyResourceGuid = new BlueprintGuid(new Guid("fb938b3d-9deb-46b3-b3a4-4de61cd2d574"));

            var demonPolyResource = Helpers.Create<BlueprintAbilityResource>(bp =>
            {
                bp.AssetGuid = demonPolyResourceGuid;
                bp.name = "DemonPolyResource" + bp.AssetGuid;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount
                {
                    BaseValue = 0,
                    IncreasedByStat = false,
                    m_ClassDiv = new BlueprintCharacterClassReference[] { demonMythicClass },
                    m_Class = new BlueprintCharacterClassReference[] { demonMythicClass },
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                    IncreasedByLevelStartPlusDivStep = true,
                    StartingLevel = 1,
                    LevelStep = 1,
                    PerStepIncrease = 1,
                    StartingIncrease = 1
                };
            });

            Helpers.AddBlueprint(demonPolyResource, demonPolyResourceGuid);

            var demonRipResourceGuid = new BlueprintGuid(new Guid("d7a3af1b-dffd-4d31-9982-71bffd04822f"));

            var demonRipResource = Helpers.Create<BlueprintAbilityResource>(bp =>
            {
                bp.AssetGuid = demonRipResourceGuid;
                bp.name = "DemonRipResource" + bp.AssetGuid;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount
                {
                    BaseValue = 2,
                    IncreasedByStat = false,
                    m_ClassDiv = new BlueprintCharacterClassReference[] { demonMythicClass },
                    m_Class = new BlueprintCharacterClassReference[] { demonMythicClass },
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                    IncreasedByLevelStartPlusDivStep = true,
                    StartingLevel = 1,
                    LevelStep = 1,
                    PerStepIncrease = 1,
                    StartingIncrease = 1
                };
            });

            Helpers.AddBlueprint(demonRipResource, demonRipResourceGuid);

            var demonTearResourceGuid = new BlueprintGuid(new Guid("3c30cf94-f6de-45a9-a799-43970fa7a2f5"));

            var demonTearResource = Helpers.Create<BlueprintAbilityResource>(bp =>
            {
                bp.AssetGuid = demonTearResourceGuid;
                bp.name = "DemonTearResource" + bp.AssetGuid;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount
                {
                    BaseValue = 2,
                    IncreasedByStat = false,
                    m_ClassDiv = new BlueprintCharacterClassReference[] { demonMythicClass },
                    m_Class = new BlueprintCharacterClassReference[] { demonMythicClass },
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                    IncreasedByLevelStartPlusDivStep = true,
                    StartingLevel = 1,
                    LevelStep = 3,
                    PerStepIncrease = 1,
                    StartingIncrease = 1
                };
            });

            Helpers.AddBlueprint(demonTearResource, demonTearResourceGuid);

        }

    }
}
