using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using WOTR_PATH_OF_RAGE.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Blueprints.Classes.Selection;
using WOTR_PATH_OF_RAGE.MechanicsChanges;

namespace WOTR_PATH_OF_RAGE.NewFeatures
{
    class AspectOfLilithu
    {
        public static void AddLilithuAspect()
        {
            var coloxusAspectActivatableAbility = BlueprintTool.Get<BlueprintActivatableAbility>("49e1df551bc9cdc499930be39a3fc8cf");
            var coloxusAspectSwitchBuff = BlueprintTool.Get<BlueprintBuff>("0e735301761c86d4184a92f18f42a1aa");
            var coloxusAspectBuff = BlueprintTool.Get<BlueprintBuff>("303e34666de545d4d8b604d720da41b4");
            var coloxusAspectFeature = BlueprintTool.Get<BlueprintFeature>("04f5985258e1d594280b5e02916a6326");

            var lilithuBuffGuid = new BlueprintGuid(new Guid("c8afe138-9f78-4c3e-b863-338026822b1e"));

            var lilithuBuff = Helpers.CreateCopy(coloxusAspectBuff, bp =>
            {
                bp.AssetGuid = lilithuBuffGuid;
                bp.m_Icon = AssetLoader.LoadInternal("Abilities", "Lilithu.png");
                bp.name = "Aspect Of Lilithu Buff" + bp.AssetGuid;
            });

            lilithuBuff.m_DisplayName = Helpers.CreateString(lilithuBuff + ".Name", "Aspect Of Lilithu");
            var lilithuBuffDescription = "Demon adopts the aspect of Lilithu, gaining a {g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Charisma}Charisma{/g} " +
                                         "{g|Encyclopedia:Ability_Scores}ability score{/g} equal to half of the Demon's mythic rank plus one.\nThe aspect of Lilithu allows " +
                                         "the Demon to cast all {g|Encyclopedia:Spell}spells{/g} or {g|Encyclopedia:Spell}spell{/g}-like abilities as if they had Selective Metamagic.\n " +
                                         "Selective Metamagic: Your allies need not fear friendly fire.\nBenefit: When casting a selective {g|Encyclopedia:Spell}spell{/g} with an area effect and a duration of instantaneous, " +
                                         "you can choose a number of targets in the area equal to the {g|Encyclopedia:Ability_Scores}ability score{/g} modifier used to determine bonus spells of the same type ({g|Encyclopedia:Charisma}Charisma{/g} for bards, " +
                                         "oracles, paladins, sorcerers, and summoners; {g|Encyclopedia:Intelligence}Intelligence{/g} for witches and wizards; {g|Encyclopedia:Wisdom}Wisdom{/g} for clerics, druids, inquisitors, and rangers). These targets are excluded " +
                                         "from the effects of your spell.\nSpells that do not have an area of effect or a duration of instantaneous do not benefit from this {g|Encyclopedia:Feat}feat{/g}.";
            lilithuBuff.m_Description = Helpers.CreateString(lilithuBuff + ".Description", lilithuBuffDescription);
            lilithuBuff.RemoveComponents<AddMechanicsFeature>();
            lilithuBuff.AddComponent<AutoMetamagic>(c =>
            {
                c.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Selective;
                c.CheckSpellbook = false;
                c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOrSpellLike;
            });

            Helpers.AddBlueprint(lilithuBuff, lilithuBuffGuid);

            ///

            var lilithuSwitchBuffGuid = new BlueprintGuid(new Guid("2a0a5451-878b-4e99-9a18-ebdba6cdbd25"));

            var lilithuSwitchBuff = Helpers.CreateCopy(coloxusAspectSwitchBuff, bp =>
            {
                bp.AssetGuid = lilithuSwitchBuffGuid;
                bp.m_Icon = lilithuBuff.m_Icon;
                bp.name = "Aspect Of Lilithu Switch Buff" + bp.AssetGuid;
            });

            lilithuSwitchBuff.m_DisplayName = lilithuBuff.m_DisplayName;
            var lilithuSwitchBuffDescription = lilithuBuff.m_Description;
            lilithuSwitchBuff.m_Description = Helpers.CreateString(lilithuSwitchBuff + ".Description", lilithuSwitchBuffDescription);

            var bee = (BuffExtraEffects)lilithuSwitchBuff.Components[0];
            bee.m_ExtraEffectBuff = lilithuBuff.ToReference<BlueprintBuffReference>();

            Helpers.AddBlueprint(lilithuSwitchBuff, lilithuSwitchBuffGuid);

            ///

            var lilithuAspectActivatableAbilityGuid = new BlueprintGuid(new Guid("397edc56-9780-45ea-b823-f09b60c3cc37"));

            var lilithuActivatableAspectAbility = Helpers.CreateCopy(coloxusAspectActivatableAbility, bp =>
            {
                bp.AssetGuid = lilithuAspectActivatableAbilityGuid;
                bp.m_Icon = lilithuBuff.m_Icon;
                bp.name = "Aspect Of Lilithu Activatable Ability" + bp.AssetGuid;
                bp.m_Buff = lilithuSwitchBuff.ToReference<BlueprintBuffReference>();
            });


            lilithuActivatableAspectAbility.m_DisplayName = lilithuBuff.m_DisplayName;
            var lilithuAspectAbilityDescription = lilithuBuff.m_Description;
            lilithuActivatableAspectAbility.m_Description = Helpers.CreateString(lilithuActivatableAspectAbility + ".Description", lilithuAspectAbilityDescription);

            Helpers.AddBlueprint(lilithuActivatableAspectAbility, lilithuAspectActivatableAbilityGuid);

            Main.Log("Lilithu Aspect Added");

            ///

            var lilithuAspectFeatureGuid = new BlueprintGuid(new Guid("55ffd907-6587-4241-b11e-f8d906cab7d7"));

            var lilithuAspectFeature = Helpers.CreateCopy(coloxusAspectFeature, bp =>
            {
                bp.AssetGuid = lilithuAspectFeatureGuid;
                bp.m_Icon = lilithuBuff.m_Icon;
                bp.name = "Aspect Of Lilithu Feature" + bp.AssetGuid;
                bp.m_DisplayName = lilithuActivatableAspectAbility.m_DisplayName;
                bp.m_Description = lilithuActivatableAspectAbility.m_Description;
            });
            var acsb = (AddContextStatBonus)lilithuAspectFeature.Components[1];
            acsb.Stat = Kingmaker.EntitySystem.Stats.StatType.Charisma;

            lilithuAspectFeature.RemoveComponents<AddFacts>();

            lilithuAspectFeature.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[]{
                        lilithuActivatableAspectAbility.ToReference<BlueprintUnitFactReference>()
                    };
            });

            Helpers.AddBlueprint(lilithuAspectFeature, lilithuAspectFeatureGuid);

            Main.Log("Lilithu Aspect Feature Created" + lilithuAspectFeatureGuid);

            if (Main.settings.AddLilithuAspect == false)
            {
                return;
            }
            var demonMajorAspectSelection = BlueprintTool.Get<BlueprintFeatureSelection>("5eba1d83a078bdd49a0adc79279e1ffe");

            demonMajorAspectSelection.AddFeatures(lilithuAspectFeature);

            Main.Log("Lilithu Aspect Added To Mythic");

        }

    }
}
