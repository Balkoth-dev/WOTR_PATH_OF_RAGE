using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOTR_PATH_OF_RAGE.MechanicsChanges
{
    // Token: 0x020019B4 RID: 6580
    [TypeId("92abd59fd9404e4f9ea0a2dfe1143332")]
    public class ContextConditionHasWeapons : ContextCondition
    {
        // Token: 0x0600AC8B RID: 44171 RVA: 0x00073792 File Offset: 0x00071992
        public override string GetConditionCaption()
        {
            if (this.CheckCaster)
            {
                return "Check if caster has an equipped shield";
            }
            return "Check if target has an equipped shield";
        }

        // Token: 0x0600AC8C RID: 44172 RVA: 0x000737A7 File Offset: 0x000719A7
        public override bool CheckCondition()
        {
            foreach (var cat in Category)
            {
                return (base.Context.MaybeCaster.Body.SecondaryHand.Weapon.Blueprint.Type.Category == cat) && (base.Context.MaybeCaster.Body.PrimaryHand.Weapon.Blueprint.Type.Category == cat);
            }
            return false;
        }

        // Token: 0x0400725D RID: 29277
        public bool CheckCaster;

        public WeaponCategory[] Category;
    }
}

