using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOTR_PATH_OF_RAGE.DemonCharge
{
    class DemonChargePatch
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;

                Main.Log("Patching Demonic Charge");
                PatchDemonicCharge();
            }

            static void PatchDemonicCharge()
            {
                if(!Main.settings.DemonicChargePointTargetting)
                {
                    return;
                }

                var demonChargeMainAbility = BlueprintTool.Get<BlueprintAbility>("1b677ed598d47a048a0f6b4b671b8f84");
                demonChargeMainAbility.CanTargetPoint = true;

            }

        }
    }
}