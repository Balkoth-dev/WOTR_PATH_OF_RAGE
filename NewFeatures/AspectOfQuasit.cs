using System;
using WOTR_PATH_OF_RAGE.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Blueprints.Classes.Selection;

namespace WOTR_PATH_OF_RAGE.NewFeatures
{
    class AspectOfQuasit
    {
        public static void AddQuasitAspect()
        {
            var coloxusAspectActivatableAbility = BlueprintTool.Get<BlueprintActivatableAbility>("49e1df551bc9cdc499930be39a3fc8cf");
            var coloxusAspectSwitchBuff = BlueprintTool.Get<BlueprintBuff>("0e735301761c86d4184a92f18f42a1aa");
            var coloxusAspectBuff = BlueprintTool.Get<BlueprintBuff>("303e34666de545d4d8b604d720da41b4");
            var coloxusAspectFeature = BlueprintTool.Get<BlueprintFeature>("04f5985258e1d594280b5e02916a6326");

            var quasitBuffGuid = new BlueprintGuid(new Guid("6b262f78-7f9d-418a-b65e-ae4c43eb6f54"));

            var quasitBuff = Helpers.CreateCopy(coloxusAspectBuff, bp =>
            {
                bp.AssetGuid = quasitBuffGuid;
                bp.m_Icon = AssetLoader.LoadInternal("Abilities", "Quasit.png");
                bp.name = "Aspect Of Quasit Buff" + bp.AssetGuid;
            });

            quasitBuff.m_DisplayName = Helpers.CreateString(quasitBuff + ".Name", "Aspect Of Quasit");
            var quasitBuffDescription = "Demon adopts the aspect of Quasit, gaining a {g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Dexterity}Dexterity{/g} " +
                                         "{g|Encyclopedia:Ability_Scores}ability score{/g} equal to half of the Demon's mythic rank plus one.\nThe aspect of Quasit allows " +
                                         "the Demon to reroll all attack rolls and take the highest result.";
            quasitBuff.m_Description = Helpers.CreateString(quasitBuff + ".Description", quasitBuffDescription);
            quasitBuff.RemoveComponents<AddMechanicsFeature>();
            quasitBuff.AddComponent<ModifyD20>(c =>
            {
                c.TakeBest = true;
                c.Rule = RuleType.AttackRoll;
                c.RollsAmount = 1;
            });

            Helpers.AddBlueprint(quasitBuff, quasitBuffGuid);

            ///

            var quasitSwitchBuffGuid = new BlueprintGuid(new Guid("9f007145-12aa-4005-9cc5-03ebfa9f877d"));

            var quasitSwitchBuff = Helpers.CreateCopy(coloxusAspectSwitchBuff, bp =>
            {
                bp.AssetGuid = quasitSwitchBuffGuid;
                bp.m_Icon = quasitBuff.m_Icon;
                bp.name = "Aspect Of Quasit Switch Buff" + bp.AssetGuid;
            });

            quasitSwitchBuff.m_DisplayName = quasitBuff.m_DisplayName;
            var quasitSwitchBuffDescription = quasitBuff.m_Description;
            quasitSwitchBuff.m_Description = Helpers.CreateString(quasitSwitchBuff + ".Description", quasitSwitchBuffDescription);

            var bee = (BuffExtraEffects)quasitSwitchBuff.Components[0];
            bee.m_ExtraEffectBuff = quasitBuff.ToReference<BlueprintBuffReference>();

            Helpers.AddBlueprint(quasitSwitchBuff, quasitSwitchBuffGuid);

            ///

            var quasitAspectActivatableAbilityGuid = new BlueprintGuid(new Guid("d2d27421-1870-482b-88f7-85598117a4ff"));

            var quasitActivatableAspectAbility = Helpers.CreateCopy(coloxusAspectActivatableAbility, bp =>
            {
                bp.AssetGuid = quasitAspectActivatableAbilityGuid;
                bp.m_Icon = quasitBuff.m_Icon;
                bp.name = "Aspect Of Quasit Activatable Ability" + bp.AssetGuid;
                bp.m_Buff = quasitSwitchBuff.ToReference<BlueprintBuffReference>();
            });


            quasitActivatableAspectAbility.m_DisplayName = quasitBuff.m_DisplayName;
            var quasitAspectAbilityDescription = quasitBuff.m_Description;
            quasitActivatableAspectAbility.m_Description = Helpers.CreateString(quasitActivatableAspectAbility + ".Description", quasitAspectAbilityDescription);

            Helpers.AddBlueprint(quasitActivatableAspectAbility, quasitAspectActivatableAbilityGuid);

            Main.Log("Quasit Aspect Added");

            ///

            var quasitAspectFeatureGuid = new BlueprintGuid(new Guid("cc6c0735-6128-4ee1-982e-5d8453a1cbf5"));

            var quasitAspectFeature = Helpers.CreateCopy(coloxusAspectFeature, bp =>
            {
                bp.AssetGuid = quasitAspectFeatureGuid;
                bp.m_Icon = quasitBuff.m_Icon;
                bp.name = "Aspect Of Quasit Feature" + bp.AssetGuid;
                bp.m_DisplayName = quasitActivatableAspectAbility.m_DisplayName;
                bp.m_Description = quasitActivatableAspectAbility.m_Description;
            });
            var acsb = (AddContextStatBonus)quasitAspectFeature.Components[1];
            acsb.Stat = Kingmaker.EntitySystem.Stats.StatType.Dexterity;

            quasitAspectFeature.RemoveComponents<AddFacts>();

            quasitAspectFeature.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[]{
                        quasitActivatableAspectAbility.ToReference<BlueprintUnitFactReference>()
                    };
            });

            Helpers.AddBlueprint(quasitAspectFeature, quasitAspectFeatureGuid);

            Main.Log("Quasit Aspect Feature Created" + quasitAspectFeatureGuid);

            if (Main.settings.AddQuasitAspect == false)
            {
                return;
            }
            var demonMajorAspectSelection = BlueprintTool.Get<BlueprintFeatureSelection>("5eba1d83a078bdd49a0adc79279e1ffe");

            demonMajorAspectSelection.AddFeatures(quasitAspectFeature);

            Main.Log("Quasit Aspect Added To Mythic");

        }

    }
}
