using System;
using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using UnityEngine;
using UnityEngine.Serialization;

namespace WOTR_PATH_OF_RAGE.MechanicsChanges
{
    // Token: 0x0200199E RID: 6558
    [TypeId("b7c4bd4f72ce4e23bfeef5b3dc7e3576")]
    public class ContextConditionCasterHasBuff : ContextCondition
    {
        // Token: 0x0600AC18 RID: 44056 RVA: 0x00073308 File Offset: 0x00071508
        public override string GetConditionCaption()
        {
            return string.Concat(new string[]
            {
                "Check if caster has buffs"
            });
        }

        // Token: 0x0600AC19 RID: 44057 RVA: 0x002F98B4 File Offset: 0x002F7AB4
        public override bool CheckCondition()
        {
            foreach (var brb in m_Buffs)
            {
                Main.Log("Looping buffs");

                foreach (Buff buff in base.Context.MaybeCaster.Buffs)
                {
                    if (buff.MaybeContext != null && (buff.Blueprint == brb))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Token: 0x0400723E RID: 29246
        [SerializeField]
        [FormerlySerializedAs("Buff")]
        public BlueprintBuff[] m_Buffs;
    }
}
