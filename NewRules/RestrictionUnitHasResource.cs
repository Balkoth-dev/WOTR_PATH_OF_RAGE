using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace WOTR_PATH_OF_RAGE.NewRules
{
    class RestrictionUnitHasResource : ActivatableAbilityRestriction
    {
        public BlueprintAbilityResourceReference m_resource;
        public override bool IsAvailable()
        {
            if (!(this.Owner == (UnitDescriptor)null))
            {
                return this.Owner.Resources.HasEnoughResource(m_resource, 1);
            }
            else
            { Main.Log("Owner Is Null"); }
            return false;
        }
    }
}
