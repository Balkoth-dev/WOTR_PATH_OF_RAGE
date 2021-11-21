using BlueprintCore.Blueprints;
using HarmonyLib;
using Kingmaker.Blueprints;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
            public bool PatchDemonAspectIcons = true;
            public bool PatchBaseRagesForDemon = true;
            public bool PatchAbyssalStorm = true;
            public bool PatchBloodHaze = true;
            public bool AddDemonBlast = true;
            public bool PatchDemonLordAspects = true;

            public override void Save(UnityModManager.ModEntry modEntry)
            {
                Save(this, modEntry);
            }
        }
        public static UnityModManager.ModEntry modInfo = null;
        public static Settings settings; 
        private static int selectedTab;
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
                    UI.Label("At Mythic Rank 4 you gain an ability to use Demonic Charge that isn't a teleport. This is for instances where you can't use Demonic Charge".green().size(10));
                }
                else
                {
                    UI.Label("Demonic Blast is not added".red().size(10));
                }
                UI.Toggle("Demon Lord Aspects Progression Fix".bold(), ref settings.PatchDemonLordAspects);
                if (settings.PatchDemonLordAspects)
                {
                    UI.Label("Demon Lord Aspects are written to be given one at 9 and another at 10. This fixes that.".green().size(10));
                }
                else
                {
                    UI.Label("Demon Lord Aspect progression is not fixed.".red().size(10));
                }
            }
        }

        private static bool Unload(UnityModManager.ModEntry arg)
        {
            throw new NotImplementedException();
        }
        public static void Log(string msg)
        {
            modInfo.Logger.Log(msg);
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
