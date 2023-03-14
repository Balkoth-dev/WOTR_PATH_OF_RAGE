using HarmonyLib;
using System;
using System.IO;
using UnityModManagerNet;
using WOTR_PATH_OF_RAGE.Utilities;
using ModKit;

namespace WOTR_PATH_OF_RAGE
{
    public class Main
    {
        public class Settings : UnityModManager.ModSettings
        {
            public bool PatchDemonRage = true;
            public bool AddDemonSpecialSelectionToMythic = true;
            public bool PatchBrimorakAspect = true;
            public bool PatchKalavakusAspect = true;
            public bool PatchShadowDemonAspect = true;
            public bool PatchSuccubusAspect = true;
            public bool PatchDemonAspectIcons = true;
            public bool PatchBaseRagesForDemon = true;
            public bool PatchAbyssalStorm = true;
            public bool PatchBloodHaze = true;
            public bool AddDemonBlast = true;
            public bool PatchDemonLordAspects = true;
            public bool AddLilithuAspect = true;
            public bool AddOolioddrooAspect = true;
            public bool PatchDemonicAura = true;
            public bool PatchAutometamagic = true;
            public bool AddMightyDemonrage = true;
            public bool PatchNocticulaAspect = true;
            public bool PatchLegendaryProportions = true;
            public bool PatchBalorAspect = true;
            public bool UncontrollableRageLocalization = true;
            public bool UncontrollableRageToggle = true;
            public bool UncontrollableRageFix = true;
            public bool DarkCodexLimitlessRageDisable = true;

            public override void Save(UnityModManager.ModEntry modEntry)
            {
                Save(this, modEntry);
            }
        }
        public static UnityModManager.ModEntry modInfo = null;
        public static Settings settings; 
        private static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            AssetLoader.ModEntry = modEntry;
            modInfo = modEntry;
            settings = Settings.Load<Settings>(modEntry);
            var settingsFile = Path.Combine(modEntry.Path, "Settings.bak");
            var copyFile = Path.Combine(modEntry.Path, "Settings.xml");
            if (File.Exists(settingsFile) && !File.Exists(copyFile))
            {
                File.Copy(settingsFile, copyFile, false);
            }
            settings = Settings.Load<Settings>(modEntry);
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            harmony.PatchAll();
            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry obj)
        {
            UI.AutoWidth(); UI.Div(0, 15);
            using (UI.VerticalScope())
            {
                UI.Label("SETTINGS WILL NOT BE UPDATED UNTIL YOU RESTART YOUR GAME.".red().bold().size(20));
                UI.Toggle("Demon Rage Rework".bold(), ref settings.PatchDemonRage);
                if(settings.PatchDemonRage)
                {
                    UI.Label("Includes a number of changes to Demon Rage. This is now a toggleable ability that will consume a number of rounds instead of per-combat and a new icon is added.".green().size(10));
                }
                else
                {
                    UI.Label("Demon Rage is unchanged. It is suggested to turn off Special Abilites as well.".red().size(10));
                }
                UI.Toggle("Allow Special Abilities".bold(), ref settings.AddDemonSpecialSelectionToMythic);
                if (settings.AddDemonSpecialSelectionToMythic)
                {
                    UI.Label("When you gain your Demon path, new special abilites called Demonologies are unlocked, allowing more rounds of rage per day.".green().size(10));
                }
                else
                {
                    UI.Label("No special abilites will be added.".red().size(10));
                }
                UI.Toggle("Brimorak Aspect Fix".bold(), ref settings.PatchBrimorakAspect);
                if (settings.PatchBrimorakAspect)
                {
                    UI.Label("Brimorak Aspect uses a new component to fix its expected scaling".green().size(10));
                }
                else
                {
                    UI.Label("Brimorak Aspect is unchanged from vanilla".red().size(10));
                }
                UI.Toggle("Succubus Aspect Fix".bold(), ref settings.PatchSuccubusAspect);
                if (settings.PatchSuccubusAspect)
                {
                    UI.Label("Succubus Aspect now properly gives a malus to Attack Bonus instead of AC".green().size(10));
                }
                else
                {
                    UI.Label("Succubus Aspect is unchanged from vanilla".red().size(10));
                }
                UI.Toggle("Kalvakus Aspect Fix".bold(), ref settings.PatchKalavakusAspect);
                if (settings.PatchKalavakusAspect)
                {
                    UI.Label("Kalvakus Aspect's bonus attack will only work while you have a melee weapon.".green().size(10));
                }
                else
                {
                    UI.Label("Kalvakus Aspect is unchanged from vanilla".red().size(10));
                }
                UI.Toggle("Shadow Demon Aspect Fix".bold(), ref settings.PatchShadowDemonAspect);
                if (settings.PatchShadowDemonAspect)
                {
                    UI.Label("Shadow Demon Aspect will no longer give double the wisdom bonus when toggled.".green().size(10));
                }
                else
                {
                    UI.Label("Shadow Demon Aspect is unchanged from vanilla".red().size(10));
                }
                UI.Toggle("Demon Aspect Icons Change".bold(), ref settings.PatchDemonAspectIcons);
                if (settings.PatchDemonAspectIcons)
                {
                    UI.Label("Adds new icons to each aspect making it easier to tell them apart.".green().size(10));
                }
                else
                {
                    UI.Label("Aspect icons are unchanged from vanilla".red().size(10));
                }
                /* UI.Toggle("Demon Lord Aspects Progression Fix".bold(), ref settings.PatchDemonLordAspects);
                  if (settings.PatchDemonLordAspects)
                  {
                      UI.Label("Demon Lord Aspects are written to be given one at 9 and another at 10. This fixes that.".green().size(10));
                  } 
                  else
                  {
                      UI.Label("Demon Lord Aspect progression is unchanged.".red().size(10));
                  } This was fixed in 1.2 */
                UI.Toggle("Allow Toggling Of Rages".bold(), ref settings.PatchBaseRagesForDemon);
                if (settings.PatchBaseRagesForDemon)
                {
                    UI.Label("Allows you to toggle Barbarian and Bloodrager rages while in a demon rage.".green().size(10));
                }
                else
                {
                    UI.Label("Barbarian and Bloodrager rages are unchanged from vanilla".red().size(10));
                }
                UI.Toggle("Abyssal Storm Fix".bold(), ref settings.PatchAbyssalStorm);
                if (settings.PatchAbyssalStorm)
                {
                    UI.Label("Abyssal Storm no longer hurts the caster.".green().size(10));
                }
                else
                {
                    UI.Label("Abyssal Storm is unchanged from vanilla".red().size(10));
                }
                UI.Toggle("Blood Haze Change".bold(), ref settings.PatchBloodHaze);
                if (settings.PatchBloodHaze)
                {
                    UI.Label("Blood Haze now gives a +2 profane bonus to attack.".green().size(10));
                }
                else
                {
                    UI.Label("Blood Haze is unchanged from vanilla".red().size(10));
                }
                UI.Toggle("Demon Blast Addition".bold(), ref settings.AddDemonBlast);
                if (settings.AddDemonBlast)
                {
                    UI.Label("At Mythic Rank 3 you gain an ability to use Demonic Charge that isn't a teleport. This is for instances where you can't use Demonic Charge".green().size(10));
                }
                else
                {
                    UI.Label("Demonic Blast is not added".red().size(10));
                }
                UI.Toggle("Lilithu Aspect Addition".bold(), ref settings.AddLilithuAspect);
                if (settings.AddLilithuAspect)
                {
                    UI.Label("As a Major demonic aspect you can select Lilithu, gaining a bonus to your Charisma as well as allowing all your spells and spell-like abilites to be considered using Selective Metamagic.".green().size(10));
                }
                else
                {
                    UI.Label("Lilithu Aspect is not added".red().size(10));
                }
                UI.Toggle("Oolioddroo Aspect Addition".bold(), ref settings.AddOolioddrooAspect);
                if (settings.AddOolioddrooAspect)
                {
                    UI.Label("As a Major demonic aspect you can select Oolioddroo, gaining a bonus to your Dexterity and allowing all attacks to be rolled twice and take the highest result.".green().size(10));
                }
                else
                {
                    UI.Label("Oolioddroo Aspect is not added".red().size(10));
                }
                UI.Toggle("Demonic Aura Fix".bold(), ref settings.PatchDemonicAura);
                if (settings.PatchDemonicAura)
                {
                    UI.Label("Fixes Demonic Aura so that it will only apply its damage when an enemy starts a new round instead of every time you activate Bloodrage.".green().size(10));
                }
                else
                {
                    UI.Label("Demonic Aura is not fixed.".red().size(10));
                }
                UI.Toggle("Autometamagic Fix".bold(), ref settings.PatchAutometamagic);
                if (settings.PatchAutometamagic)
                {
                    UI.Label("Fixes autometamagic so that it will work on spells that are not originally part of a spellbook but added later. This is important for Arcane Bloodline for Bloodragers.".green().size(10));
                }
                else
                {
                    UI.Label("Autometamagic is not fixed.".red().size(10));
                }
                UI.Toggle("Mighty Demonrage addition".bold(), ref settings.AddMightyDemonrage);
                if (settings.AddMightyDemonrage)
                {
                    UI.Label("Adds a new mythic ability called Mighty Demonrage. This allows you to cast a demon spell of fourth level or lower as a swift action while Demon Raging.".green().size(10));
                }
                else
                {
                    UI.Label("Mighty Demonrage is not added.".red().size(10));
                }
                UI.Toggle("Patch Nocticula Aspect".bold(), ref settings.PatchNocticulaAspect);
                if (settings.PatchNocticulaAspect)
                {
                    UI.Label("Changes Nocticula Aspect to have a unique icon icon. Changes the aspect to give bloodrage/barbarian rage like Channel Rage does. Changes to only use once per combat.".green().size(10));
                }
                else
                {
                    UI.Label("Nocticula Aspect is not changed.".red().size(10));
                }
                UI.Toggle("Patch Legendary Proportions".bold(), ref settings.PatchLegendaryProportions);
                if (settings.PatchLegendaryProportions)
                {
                    UI.Label("Changes the Legendary Proportions to increase your size to Large instead of Huge. This is an annoying buff to use when you use Abyssal Bulk.".green().size(10));
                }
                else
                {
                    UI.Label("Legendary Proportions is not changed.".red().size(10));
                }
                UI.Toggle("Patch Balor Aspect".bold(), ref settings.PatchBalorAspect);
                if (settings.PatchBalorAspect)
                {
                    UI.Label("Fixes Balor Aspect to not give a bonus on intelligence.".green().size(10));
                }
                else
                {
                    UI.Label("Balor Aspect is unchanged.".red().size(10));
                }
                UI.Toggle("Patch Uncontrollable Rage Localization".bold(), ref settings.UncontrollableRageLocalization);
                if (settings.UncontrollableRageLocalization)
                {
                    UI.Label("Fixes Uncontrollable Rage's localization to have an actual description and name".green().size(10));
                }
                else
                {
                    UI.Label("Uncontrollable Rage's localization is not changed.".red().size(10));
                }
                UI.Toggle("Turn Uncontrollable Rage to Toggle".bold(), ref settings.UncontrollableRageToggle);
                if (settings.UncontrollableRageToggle)
                {
                    UI.Label("Changes Uncontrollable Rage to be a toggle, instead of a feature.".green().size(10));
                }
                else
                {
                    UI.Label("Uncontrollable Rage is not turned into a toggle.".red().size(10));
                }
                UI.Toggle("Fix Uncontrollable Rage".bold(), ref settings.UncontrollableRageToggle);
                if (settings.UncontrollableRageToggle)
                {
                    UI.Label("Fixes Uncontrollable Rage so that it does what the description says.".green().size(10));
                }
                else
                {
                    UI.Label("Uncontrollable Rage is unfixed.".red().size(10));
                }
                UI.Toggle("Dark Codex Limitless Rage".bold(), ref settings.DarkCodexLimitlessRageDisable);
                if (settings.DarkCodexLimitlessRageDisable)
                {
                    UI.Label("Removes the limitless rage resource that Dark Codex gives to Demon Rage. This only affects the new Demon Rage.".green().size(10));
                }
                else
                {
                    UI.Label("Dark Codex will still give demonic rage limitless rage.".red().size(10));
                }
            }
        }

        private static bool Unload(UnityModManager.ModEntry arg)
        {
            throw new NotImplementedException();
        }
        public static void Log(string msg)
        {
#if DEBUG
            modInfo.Logger.Log(msg);
#endif
        }

        public bool GetSettingValue(string b)
        {
            return true;
        }
        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;

            return true;
        }

    }
}
