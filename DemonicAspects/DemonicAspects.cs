using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.Abilities;
using BlueprintCore;
using BlueprintCore.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Blueprints;
using WOTR_PATH_OF_RAGE.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using WOTR_PATH_OF_RAGE.Utilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Classes.Spells;
using WOTR_PATH_OF_RAGE.MechanicsChanges;

namespace WOTR_PATH_OF_RAGE.DemonRage
{
    class DemonicAspects
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;

                Main.Log("Patching Demonic Aspects");
                PatchDemonAspectIcons();
                PatchBrimorakAspect();
                PatchKalavakusAspect();
                PatchShadowDemonAspect();
                PatchSuccubusDemonAspect();
            }

            static void PatchDemonAspectIcons()
            {
                if(Main.settings.PatchDemonAspectIcons == false)
                {
                    return;
                }
                var babauAspectIcon = AssetLoader.LoadInternal("Abilities", "Baubau.png");
                var brimorakAspectIcon = AssetLoader.LoadInternal("Abilities", "Brimorak.png");
                var incubusAspectIcon = AssetLoader.LoadInternal("Abilities", "Incubus.png");
                var kalavakusAspectIcon = AssetLoader.LoadInternal("Abilities", "Kalavakus.png");
                var nabasuAspectIcon = AssetLoader.LoadInternal("Abilities", "Nabasu.png");
                var schirAspectIcon = AssetLoader.LoadInternal("Abilities", "Schir.png");
                var succubusAspectIcon = AssetLoader.LoadInternal("Abilities", "Succubus.png");
                var vrockAspectIcon = AssetLoader.LoadInternal("Abilities", "Vrock.png");
                var balorAspectIcon = AssetLoader.LoadInternal("Abilities", "Balor.png");
                var coloxusAspectIcon = AssetLoader.LoadInternal("Abilities", "Coloxus.png");
                var omoxAspectIcon = AssetLoader.LoadInternal("Abilities", "Omox.png");
                var shadowDemonAspectIcon = AssetLoader.LoadInternal("Abilities", "ShadowDemon.png");
                var vavakiaAspectIcon = AssetLoader.LoadInternal("Abilities", "Vavakia.png");
                var vrolikaiAspectIcon = AssetLoader.LoadInternal("Abilities", "Vrolikai.png");
                var areshkagalAspectIcon = AssetLoader.LoadInternal("Abilities", "Areshkagal.png");
                var deskariAspectIcon = AssetLoader.LoadInternal("Abilities", "Deskari.png");
                var kabririAspectIcon = AssetLoader.LoadInternal("Abilities", "Kabriri.png");
                var nocticulaAspectIcon = AssetLoader.LoadInternal("Abilities", "Nocticula.png");
                var pazuzuAspectIcon = AssetLoader.LoadInternal("Abilities", "Pazuzu.png");
                var socothbenothAspectIcon = AssetLoader.LoadInternal("Abilities", "Socothbenoth.png");
                ///
                var babauAspectFeature = BlueprintTool.Get<BlueprintFeature>("99a34a0fa0c3a154fbc5b11fe2d18009");
                var brimorakAspectFeature = BlueprintTool.Get<BlueprintFeature>("28f08ce20ee81b24abfd49404e4b9577");
                var incubusAspectFeature = BlueprintTool.Get<BlueprintFeature>("64c6b4e233e718d429dab47eb3a25ac6");
                var kalavakusAspectFeature = BlueprintTool.Get<BlueprintFeature>("4f94ea9a5c2779a458263976dfa02340");
                var nabasuAspectFeature = BlueprintTool.Get<BlueprintFeature>("cadc545dd171fb54ab4e62f8f4e0b670");
                var schirAspectFeature = BlueprintTool.Get<BlueprintFeature>("90f1347b3f40dc94aa254235cb8bcda2");
                var succubusAspectFeature = BlueprintTool.Get<BlueprintFeature>("709ae9be4d75f094f84f26bccb07351d");
                var vrockAspectFeature = BlueprintTool.Get<BlueprintFeature>("38a54d808e7e1d8479d309a7e8b0ab3e");
                var balorAspectFeature = BlueprintTool.Get<BlueprintFeature>("45507637ea8b28948a21c9ecbfa664e4");
                var coloxusAspectFeature = BlueprintTool.Get<BlueprintFeature>("04f5985258e1d594280b5e02916a6326");
                var omoxAspectFeature = BlueprintTool.Get<BlueprintFeature>("daf030c7563b2664eb1031d91eaae7ab");
                var shadowDemonAspectFeature = BlueprintTool.Get<BlueprintFeature>("18d0dafd5a1f8c043b5f01539f7b096c");
                var vavakiaAspectFeature = BlueprintTool.Get<BlueprintFeature>("3df57316028799448aa1b18cad1ad939");
                var vrolikaiAspectFeature = BlueprintTool.Get<BlueprintFeature>("0ed608f1a0695cd4cb80bf6d415ab295");
                var areshkagalAspectFeature = BlueprintTool.Get<BlueprintFeature>("a4e7223c6d8a7b941b9464ae73b59bcc");
                var deskariAspectFeature = BlueprintTool.Get<BlueprintFeature>("60f57fe80fa1986478f474c0fb5e90ac");
                var kabririAspectFeature = BlueprintTool.Get<BlueprintFeature>("043932e79c20f1547b3f20499df7dd7a");
                var nocticulaAspectFeature = BlueprintTool.Get<BlueprintFeature>("d48e0039bcfde694d8691418d12ebea1");
                var pazuzuAspectFeature = BlueprintTool.Get<BlueprintFeature>("fd126181b5886e54e88d9f7ed09d077b");
                var socothbenothAspectFeature = BlueprintTool.Get<BlueprintFeature>("089746782a827754cbdaf18d5ea6a5a2");
                ///
                babauAspectFeature.m_Icon = babauAspectIcon;
                brimorakAspectFeature.m_Icon = brimorakAspectIcon;
                incubusAspectFeature.m_Icon = incubusAspectIcon;
                kalavakusAspectFeature.m_Icon = kalavakusAspectIcon;
                nabasuAspectFeature.m_Icon = nabasuAspectIcon;
                schirAspectFeature.m_Icon = schirAspectIcon;
                succubusAspectFeature.m_Icon = succubusAspectIcon;
                vrockAspectFeature.m_Icon = vrockAspectIcon;
                balorAspectFeature.m_Icon = balorAspectIcon;
                coloxusAspectFeature.m_Icon = coloxusAspectIcon;
                omoxAspectFeature.m_Icon = omoxAspectIcon;
                shadowDemonAspectFeature.m_Icon = shadowDemonAspectIcon;
                vavakiaAspectFeature.m_Icon = vavakiaAspectIcon;
                vrolikaiAspectFeature.m_Icon = vrolikaiAspectIcon;
                areshkagalAspectFeature.m_Icon = areshkagalAspectIcon;
                deskariAspectFeature.m_Icon = deskariAspectIcon;
                kabririAspectFeature.m_Icon = kabririAspectIcon;
                nocticulaAspectFeature.m_Icon = nocticulaAspectIcon;
                pazuzuAspectFeature.m_Icon = pazuzuAspectIcon;
                socothbenothAspectFeature.m_Icon = socothbenothAspectIcon;
                //////
                var babauAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("e24fbd97558f06b45a09c7fbe7170a55");
                var brimorakAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("e642444d21a4dab4ea07cd042e6f9dc0");
                var incubusAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("55c6e91192b92b8479fa66d6aee33074");
                var kalavakusAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("37bfe9e5535e54c49b248bd84305ebd5");
                var nabasuAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("868c4957c5671114eaaf8e0b6b55ad3f");
                var schirAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("fae00e8f4de9cd54da800d383ede7812");
                var succubusAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("8a474cae6e2788a498f616d36b56b5d2");
                var vrockAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("600cf1ff1d381d8488faed4e7fbda865");
                var balorAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("3070984d4c8bd4f48877337da6c7535d");
                var coloxusAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("49e1df551bc9cdc499930be39a3fc8cf");
                var omoxAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("e305991cb9461a04a97e4f5b02b8b767");
                var shadowDemonAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("0d817aa4f8bc00541a43ea2f822d124b");
                var vavakiaAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("b6dc815e86a12654eb7f78c5f14008df");
                var vrolikaiAspectAbility = BlueprintTool.Get<BlueprintActivatableAbility>("df9e7bbc606b0cd4087ee2d08cc2c09b");
                ///
                babauAspectAbility.m_Icon = babauAspectIcon;
                brimorakAspectAbility.m_Icon = brimorakAspectIcon;
                incubusAspectAbility.m_Icon = incubusAspectIcon;
                kalavakusAspectAbility.m_Icon = kalavakusAspectIcon;
                nabasuAspectAbility.m_Icon = nabasuAspectIcon;
                schirAspectAbility.m_Icon = schirAspectIcon;
                succubusAspectAbility.m_Icon = succubusAspectIcon;
                vrockAspectAbility.m_Icon = vrockAspectIcon;
                balorAspectAbility.m_Icon = balorAspectIcon;
                coloxusAspectAbility.m_Icon = coloxusAspectIcon;
                omoxAspectAbility.m_Icon = omoxAspectIcon;
                shadowDemonAspectAbility.m_Icon = shadowDemonAspectIcon;
                vavakiaAspectAbility.m_Icon = vavakiaAspectIcon;
                vrolikaiAspectAbility.m_Icon = vrolikaiAspectIcon;
                ///

                Main.Log("Patching Aspect Icons Complete");
            }
        }

        static void PatchBrimorakAspect()
        {
            if (Main.settings.PatchBrimorakAspect == false)
            {
                return;
            }
            var brimorakAspectEffectBuff = BlueprintTool.Get<BlueprintBuff>("f154542e0b97908479a578dd7bf6d3f7");
            brimorakAspectEffectBuff.RemoveComponents<ContextRankConfig>();
            brimorakAspectEffectBuff.AddComponent<ContextRankConfig>(c =>
            {
                c.m_Type = AbilityRankType.StatBonus;
                c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                c.m_StepLevel = 3;
                c.m_Max = 20;
                c.m_Progression = ContextRankProgression.Div2PlusStep;
            });
            brimorakAspectEffectBuff.RemoveComponents<DraconicBloodlineArcana>();
            brimorakAspectEffectBuff.AddComponent<BrimorakAspectDamage>();
            Main.Log("Patching Brimorak Aspect Complete");
        }

        static void PatchKalavakusAspect()
        {
            if (Main.settings.PatchKalavakusAspect == false)
            {
                return;
            }
            var kalavakusAspectEffectBuff = BlueprintTool.Get<BlueprintBuff>("c9cdd3af3c5f93c4fb8e9119adaa582e");
            kalavakusAspectEffectBuff.EditComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
            c.RangeType = WeaponRangeType.Melee);
            Main.Log("Patching Brimorak Aspect Complete");
        }
        static void PatchShadowDemonAspect()
        {
            if (Main.settings.PatchShadowDemonAspect == false)
            {
                return;
            }
            var shadowDemonAspectSwitchBuff = BlueprintTool.Get<BlueprintBuff>("d5336d599d004e74d9af6b8967c3f217");
            shadowDemonAspectSwitchBuff.RemoveComponents<AddContextStatBonus>();
            Main.Log("Patching Shadow Demon Aspect Complete");
        }
        static void PatchSuccubusDemonAspect()
        {
            if (Main.settings.PatchSuccubusAspect == false)
            {
                return;
            }
            var succubusAspectEnemyBuff = BlueprintTool.Get<BlueprintBuff>("5a350c892f24f4f4880b93805be6c89b");
            var succubusAspectContextStatBonus = (AddContextStatBonus)succubusAspectEnemyBuff.Components[1];
            succubusAspectContextStatBonus.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus;
            Main.Log("Patching Succubus Aspect Complete");
        }

    }
}
