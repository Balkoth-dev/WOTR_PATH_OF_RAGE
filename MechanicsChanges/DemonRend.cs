using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.ContextData;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOTR_PATH_OF_RAGE.MechanicsChanges
{
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("d797cb4e36334b03855eeb1b1bd6f761")]
    public class DemonRend
    : UnitFactComponentDelegate, IRulebookHandler<RuleAttackWithWeapon>, IInitiatorRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        // Token: 0x0600C232 RID: 49714 RVA: 0x0007F962 File Offset: 0x0007DB62
        public override void OnTurnOn()
        {
            base.Owner.State.Features.HasRend.Retain();
        }

        // Token: 0x0600C233 RID: 49715 RVA: 0x0007F97E File Offset: 0x0007DB7E
        public override void OnTurnOff()
        {
            base.Owner.State.Features.HasRend.Release();
        }

        // Token: 0x0600C234 RID: 49716 RVA: 0x000031E7 File Offset: 0x000013E7
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
        }
        private static void RunActions(
        DemonRend c,
        RuleAttackWithWeapon rule,
        MechanicsContext context,
        EntityFact fact)
        {
            UnitEntityData unitEntityData = c.ActionsOnInitiator ? rule.Initiator : rule.Target;
            using (Kingmaker.ElementsSystem.ContextData<ContextAttackData>.Request().Setup(rule.AttackRoll))
            {
                if (fact.IsDisposed)
                {
                    using (context.GetDataScope((TargetWrapper)unitEntityData))
                        c.Action.Run();
                }
                else
                    fact.RunActionInContext(c.Action, (TargetWrapper)unitEntityData);
            }
        }

        // Token: 0x0600C235 RID: 49717 RVA: 0x00338544 File Offset: 0x00336744
        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            Main.Log("Event Trigger");
            try
            {
                bool hasBuff = false;
                bool hasBothWeapons = false;
                foreach (Buff buff in base.Context.MaybeCaster.Buffs)
                {
                    if (buff.MaybeContext != null && (buff.MaybeContext.SpellDescriptor & this.SpellDescriptor) != Kingmaker.Blueprints.Classes.Spells.SpellDescriptor.None)
                    {
                        hasBuff = true;
                        break;
                    }
                }

                foreach (var cat in Category)
                {
                    hasBothWeapons = (base.Context.MaybeCaster.Body.SecondaryHand.Weapon.Blueprint.Type.Category == cat) && (base.Context.MaybeCaster.Body.PrimaryHand.Weapon.Blueprint.Type.Category == cat);
                }

                if (evt.IsRend && evt.AttackRoll.IsHit && hasBuff == false && hasBothWeapons == true)
                {
                    if (UseMythicLevel)
                    {
                        var mythicLevel = base.Owner.Progression.MythicLevel*2;
                        this.RendDamage.m_Rolls = mythicLevel;
                    }
                    int bonus = 0;
                    if (ApplyStrengthBonus)
                    {
                        bonus = (int)((double)base.Owner.Stats.Strength.Bonus * 1.5);
                    }

                    BaseDamage damage = this.RendType.GetDamageDescriptor(this.RendDamage, bonus).CreateDamage();
                    RuleDealDamage evt2 = new RuleDealDamage(base.Owner, evt.Target, damage);
                    Game.Instance.Rulebook.TriggerEvent<RuleDealDamage>(evt2);

                    MechanicsContext context = this.Context;
                    EntityFact fact = this.Fact;
                    RunActions(this, evt, context, fact);
                }

            }
            catch (Exception ex)
            {
                Main.Log(ex.ToString());
            }
        }

        // Token: 0x04007F09 RID: 32521
        public DiceFormula RendDamage;

        // Token: 0x04007F0A RID: 32522
        public DamageTypeDescription RendType;

        public SpellDescriptorWrapper SpellDescriptor;

        public ActionList Action;

        public bool ActionsOnInitiator;

        public bool UseMythicLevel;

        public bool ApplyStrengthBonus;

        public WeaponCategory[] Category;
    };

}
