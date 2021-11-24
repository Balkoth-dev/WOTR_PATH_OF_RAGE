using BlueprintCore.Blueprints;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOTR_PATH_OF_RAGE.Utilities;
using BlueprintCore.Utils;

namespace WOTR_PATH_OF_RAGE.NewFeatures
{
    class DemonSmashProjectile
    {
        public static void AddDemonSmashProjectile()
        {
            var fireball00 = BlueprintTool.Get<BlueprintProjectile>("8927afa172e0fc54484a29fa0c4c40c4");
            var demonChargeProjectile = BlueprintTool.Get<BlueprintAbility>("4b18d0f44f57cbf4c91f094addfed9f4");
            var mythic5lvlDemon_DemonicCharge00_Projectile = BlueprintTool.Get<BlueprintProjectile>("1d53a06c3b2aa434890f8e9d63fee7bd");
            var dummy_projectile = BlueprintTool.Get<BlueprintProjectile>("b8e4b2d648683fb43b8a60d2bf36d2b2");

            var demonSmashProGuid = new BlueprintGuid(new Guid("fec53329-817f-4aa6-a921-0be9867a8930"));

            var demonSmashProjectile = Helpers.CreateCopy(fireball00, bp =>
            {
                bp.AssetGuid = demonSmashProGuid;
            });
            demonSmashProjectile.View = dummy_projectile.View;
            demonSmashProjectile.ProjectileHit.HitFx = demonChargeProjectile.GetComponent<AbilitySpawnFx>().PrefabLink;
            demonSmashProjectile.ProjectileHit.MissFx = demonChargeProjectile.GetComponent<AbilitySpawnFx>().PrefabLink;

            Helpers.AddBlueprint(demonSmashProjectile, demonSmashProGuid);
            Main.Log("Demonic Projectile Added " + demonSmashProjectile.AssetGuid.ToString());
        }
    }
}
