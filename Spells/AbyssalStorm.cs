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
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace WOTR_PATH_OF_RAGE.Spells
{
    class AbyssalStorm
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;

                Main.Log("Patching Abyssal Storm");
                PatchAbyssalStorm();
            }
            static void PatchAbyssalStorm()
            {
                if (Main.settings.PatchAbyssalStorm == false)
                {
                    return;
                }
                var abyssalStorm = BlueprintTool.Get<BlueprintAbility>("58e9e2883bca1574e9c932e72fd361f9");

                abyssalStorm.EditComponent<AbilityTargetsAround>(c =>
                {
                    c.m_Condition = new ConditionsChecker()
                    {
                        Conditions = new Condition[] {
                        new ContextConditionIsCaster() {
                            Not = true
                        }
                    }
                    };
                });

                Main.Log("Abyssal Storm Patched");
            }
        }
    }
}
