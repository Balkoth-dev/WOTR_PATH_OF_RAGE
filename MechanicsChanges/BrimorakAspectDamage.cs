using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;

namespace WOTR_PATH_OF_RAGE.MechanicsChanges
{
    [TypeId("a584568f53d2454a97a48123d6f7d78f")]
    public class BrimorakAspectDamage : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            MechanicsContext context = evt.Reason.Context;
            if((context != null ? context.SourceAbility : null) == null)
            {
                return;
            }
            if(!context.SourceAbility.IsSpell)
            {
                return;
            }
            foreach(BaseDamage baseDamage in evt.DamageBundle)
            {
                if (!baseDamage.Precision)
                {
                    DiceFormula modifiedValue = baseDamage.Dice.ModifiedValue;
                    int bonus = this.UseContextBonus ? this.Value.Calculate(context) * modifiedValue.Rolls : modifiedValue.Rolls;
                    baseDamage.AddModifier(bonus, base.Fact);
                }
            }
        }
        public void OnEventDidTrigger(RuleCalculateDamage evt)
        {

        }

        public bool UseContextBonus;
        public ContextValue Value;
    }
}