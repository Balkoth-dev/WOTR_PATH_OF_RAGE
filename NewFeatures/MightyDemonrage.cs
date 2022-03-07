using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using WOTR_PATH_OF_RAGE.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Utils;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Prerequisites;

namespace WOTR_PATH_OF_RAGE.NewFeatures
{
    class MightyDemonrage
    {
        public static void AddMightyDemonrage()
        {
            var bloodragerMightyBloodragerBuff = BlueprintTool.Get<BlueprintBuff>("b8a8d387f4bd46b5b283089f5ff0ec61");
            var demonSpellbook = BlueprintTool.Get<BlueprintSpellbook>("e3daa889c72982e45a026f62cc84937d");

            var demonMightyDemonrageBuffGuid = new BlueprintGuid(new Guid("d29e501d-4483-4fae-90f9-ffa254bb1644"));

            var demonMightyDemonrageBuff = Helpers.CreateCopy(bloodragerMightyBloodragerBuff, bp =>
            {
                bp.AssetGuid = demonMightyDemonrageBuffGuid;
                bp.name = "Mighty Demon Rage"+bp.AssetGuid;
                bp.m_DisplayName = Helpers.CreateString(bp + ".Name", "Mighty Demon Rage");
                bp.m_Description = Helpers.CreateString(bp + ".Description", "Once per Demon Rage you may cast any spell fourth level or lower from the Demon spellbook as a swift action.");
                bp.m_Icon = AssetLoader.LoadInternal("Abilities", "MightyDemonRage.png");
            });

            demonMightyDemonrageBuff.EditComponent<AutoMetamagic>(c =>
            {
                c.m_Spellbook = demonSpellbook.ToReference<BlueprintSpellbookReference>();
                c.Once = true;
                c.MaxSpellLevel = 4;
            });

            Helpers.AddBlueprint(demonMightyDemonrageBuff, demonMightyDemonrageBuffGuid);

            Main.Log("Mighty Demonrage Buff Added");

            var bloodragerMightyBloodragerFeature = BlueprintTool.Get<BlueprintFeature>("a6cd3eca05ee24840ab159ca47b4cd88");

            var demonMightyDemonrageFeatureGuid = new BlueprintGuid(new Guid("1f060f90-57a2-40b7-a11a-8d5987ef7a20"));

            var demonMightyDemonrageFeature = Helpers.CreateCopy(bloodragerMightyBloodragerFeature, bp =>
            {
                bp.AssetGuid = demonMightyDemonrageFeatureGuid;
                bp.name = "Mighty Demon Rage" + bp.AssetGuid;
                bp.m_DisplayName = Helpers.CreateString(bp + ".Name", "Mighty Demon Rage");
                bp.m_Description = Helpers.CreateString(bp + ".Description", "Once per Demon Rage you may cast any spell fourth level or lower from the Demon spellbook as a swift action.");
                bp.m_Icon = AssetLoader.LoadInternal("Abilities", "MightyDemonRage.png");
            });

            var demonRageBuff = BlueprintTool.Get<BlueprintBuff>("36ca5ecd8e755a34f8da6b42ad4c965f");
            var demonClass = BlueprintTool.Get<BlueprintCharacterClass>("8e19495ea576a8641964102d177e34b7");

            demonMightyDemonrageFeature.EditComponent<BuffExtraEffects>(c =>
            {
                c.m_CheckedBuff = demonRageBuff.ToReference<BlueprintBuffReference>();
                c.m_ExtraEffectBuff = demonMightyDemonrageBuff.ToReference<BlueprintBuffReference>();
            });

            demonMightyDemonrageFeature.AddComponent<PrerequisiteClassLevel> (c =>
            {
                c.m_CharacterClass = demonClass.ToReference<BlueprintCharacterClassReference>();
                c.Level = 1;
            });

            Helpers.AddBlueprint(demonMightyDemonrageFeature, demonMightyDemonrageFeatureGuid);


            Main.Log("Mighty Demonrage Feature Added");

            if (!Main.settings.AddMightyDemonrage)
            { return; }

            var mythicAbilitySelection = BlueprintTool.Get<BlueprintFeatureSelection>("ba0e5a900b775be4a99702f1ed08914d");

            mythicAbilitySelection.AddFeatures(demonMightyDemonrageFeature);

            Main.Log("Mighty Demonrage Feature Added To Mythic Selection");



        }

    }
}
