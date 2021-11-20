using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using UnityEngine;
using static Kingmaker.Blueprints.Area.FactHolder;

namespace WOTR_PATH_OF_RAGE.MechanicsChanges
{
    [AllowMultipleComponents]
    [ComponentName("Increase spell school DC")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("86e3be2acefa421398faa86dead30b83")]
    public class IncreaseSpelllistDC : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            if (evt.AbilityData.IsInSpellList(m_SpellList))
            {
                int num = this.BonusDC * base.Fact.GetRank();
                evt.AddBonusDC(num, this.Descriptor);
            }
        }

        public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {
        }

        [SerializeField]
        public BlueprintSpellListReference m_SpellList;

        public int BonusDC;

        public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;
    }
}