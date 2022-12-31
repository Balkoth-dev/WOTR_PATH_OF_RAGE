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
using WOTR_PATH_OF_RAGE.MechanicsChanges;

namespace WOTR_PATH_OF_RAGE.NewFeatures
{
    class AspectOfOolioddroo
    {
        public static void AddOolioddrooAspect()
        {
            var coloxusAspectActivatableAbility = BlueprintTool.Get<BlueprintActivatableAbility>("49e1df551bc9cdc499930be39a3fc8cf");
            var coloxusAspectSwitchBuff = BlueprintTool.Get<BlueprintBuff>("0e735301761c86d4184a92f18f42a1aa");
            var coloxusAspectBuff = BlueprintTool.Get<BlueprintBuff>("303e34666de545d4d8b604d720da41b4");
            var coloxusAspectFeature = BlueprintTool.Get<BlueprintFeature>("04f5985258e1d594280b5e02916a6326");

            var oolioddrooBuffGuid = new BlueprintGuid(new Guid("6b262f78-7f9d-418a-b65e-ae4c43eb6f54"));

            var oolioddrooBuff = Helpers.CreateCopy(coloxusAspectBuff, bp =>
            {
                bp.AssetGuid = oolioddrooBuffGuid;
                bp.m_Icon = AssetLoader.LoadInternal("Abilities", "Oolioddroo.png");
                bp.name = "Aspect Of oolioddroo Buff" + bp.AssetGuid;
            });

            oolioddrooBuff.m_DisplayName = Helpers.CreateString(oolioddrooBuff + ".Name", "Aspect Of Oolioddroo");
            var oolioddrooBuffDescription = "Demon adopts the aspect of Oolioddroo, gaining a {g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Dexterity}Dexterity{/g} " +
                                         "{g|Encyclopedia:Ability_Scores}ability score{/g} equal to half of the Demon's mythic rank plus one.\nThe aspect of oolioddroo allows " +
                                         "the Demon to reroll all attack rolls and concealment checks that fail.";
            oolioddrooBuff.m_Description = Helpers.CreateString(oolioddrooBuff + ".Description", oolioddrooBuffDescription);
            oolioddrooBuff.RemoveComponents<AddMechanicsFeature>();
            oolioddrooBuff.AddComponent<ModifyD20>(c =>
             {
                 c.TakeBest = true;
                 c.Rule = RuleType.AttackRoll;
                 c.RollsAmount = 1;
                 c.RerollOnlyIfFailed = true;
             });
            oolioddrooBuff.AddComponent<RerollConcealment>();

            Helpers.AddBlueprint(oolioddrooBuff, oolioddrooBuffGuid);

            ///

            var oolioddrooSwitchBuffGuid = new BlueprintGuid(new Guid("9f007145-12aa-4005-9cc5-03ebfa9f877d"));

            var oolioddrooSwitchBuff = Helpers.CreateCopy(coloxusAspectSwitchBuff, bp =>
            {
                bp.AssetGuid = oolioddrooSwitchBuffGuid;
                bp.m_Icon = oolioddrooBuff.m_Icon;
                bp.name = "Aspect Of oolioddroo Switch Buff" + bp.AssetGuid;
            });

            oolioddrooSwitchBuff.m_DisplayName = oolioddrooBuff.m_DisplayName;
            var oolioddrooSwitchBuffDescription = oolioddrooBuff.m_Description;
            oolioddrooSwitchBuff.m_Description = Helpers.CreateString(oolioddrooSwitchBuff + ".Description", oolioddrooSwitchBuffDescription);

            var bee = (BuffExtraEffects)oolioddrooSwitchBuff.Components[0];
            bee.m_ExtraEffectBuff = oolioddrooBuff.ToReference<BlueprintBuffReference>();

            Helpers.AddBlueprint(oolioddrooSwitchBuff, oolioddrooSwitchBuffGuid);

            ///

            var oolioddrooAspectActivatableAbilityGuid = new BlueprintGuid(new Guid("d2d27421-1870-482b-88f7-85598117a4ff"));

            var oolioddrooActivatableAspectAbility = Helpers.CreateCopy(coloxusAspectActivatableAbility, bp =>
            {
                bp.AssetGuid = oolioddrooAspectActivatableAbilityGuid;
                bp.m_Icon = oolioddrooBuff.m_Icon;
                bp.name = "Aspect Of oolioddroo Activatable Ability" + bp.AssetGuid;
                bp.m_Buff = oolioddrooSwitchBuff.ToReference<BlueprintBuffReference>();
            });


            oolioddrooActivatableAspectAbility.m_DisplayName = oolioddrooBuff.m_DisplayName;
            var oolioddrooAspectAbilityDescription = oolioddrooBuff.m_Description;
            oolioddrooActivatableAspectAbility.m_Description = Helpers.CreateString(oolioddrooActivatableAspectAbility + ".Description", oolioddrooAspectAbilityDescription);

            Helpers.AddBlueprint(oolioddrooActivatableAspectAbility, oolioddrooAspectActivatableAbilityGuid);

            Main.Log("Oolioddroo Aspect Added");

            ///

            var oolioddrooAspectFeatureGuid = new BlueprintGuid(new Guid("cc6c0735-6128-4ee1-982e-5d8453a1cbf5"));

            var oolioddrooAspectFeature = Helpers.CreateCopy(coloxusAspectFeature, bp =>
            {
                bp.AssetGuid = oolioddrooAspectFeatureGuid;
                bp.m_Icon = oolioddrooBuff.m_Icon;
                bp.name = "Aspect Of oolioddroo Feature" + bp.AssetGuid;
                bp.m_DisplayName = oolioddrooActivatableAspectAbility.m_DisplayName;
                bp.m_Description = oolioddrooActivatableAspectAbility.m_Description;
            });
            var acsb = (AddContextStatBonus)oolioddrooAspectFeature.Components[1];
            acsb.Stat = Kingmaker.EntitySystem.Stats.StatType.Dexterity;

            oolioddrooAspectFeature.RemoveComponents<AddFacts>();

            oolioddrooAspectFeature.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[]{
                        oolioddrooActivatableAspectAbility.ToReference<BlueprintUnitFactReference>()
                    };
            });

            Helpers.AddBlueprint(oolioddrooAspectFeature, oolioddrooAspectFeatureGuid);

            Main.Log("oolioddroo Aspect Feature Created" + oolioddrooAspectFeatureGuid);

            if (Main.settings.AddOolioddrooAspect == false)
            {
                return;
            }
            var demonMajorAspectSelection = BlueprintTool.Get<BlueprintFeatureSelection>("5eba1d83a078bdd49a0adc79279e1ffe");

            demonMajorAspectSelection.AddFeatures(oolioddrooAspectFeature);

            Main.Log("Oolioddroo Aspect Added To Mythic");

            var nocticulaAspectBuff = BlueprintTool.Get<BlueprintBuff>("ef035e3fee135504ebfe9d0d052762f8");
            nocticulaAspectBuff.GetComponent<AddFactsFromCaster>().m_Facts = nocticulaAspectBuff.GetComponent<AddFactsFromCaster>().m_Facts.AppendToArray(oolioddrooSwitchBuff.ToReference<BlueprintUnitFactReference>());


        }

    }
}
