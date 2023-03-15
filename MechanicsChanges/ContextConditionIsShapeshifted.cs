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
    public class ContextConditionIsShapeshifted : ContextCondition
    {
        public override string GetConditionCaption()
        {
            return string.Concat(new string[]
            {
                "Check if caster is shapeshifted"
            });
        }

        public override bool CheckCondition()
        {
            bool isPolymorphed = base.Context.MaybeCaster.Body.IsPolymorphed;
            bool isKeepSlots = base.Context.MaybeCaster.Body.IsPolymorphKeepSlots;

            if (isPolymorphed || isKeepSlots)
            {
                return true;
            }

            return false;
        }
    }
}
