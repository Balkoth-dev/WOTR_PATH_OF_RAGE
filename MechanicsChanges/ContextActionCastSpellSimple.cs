using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace WOTR_PATH_OF_RAGE.MechanicsChanges
{
	// Token: 0x0200192F RID: 6447
	[TypeId("2fbdac84837e4ddd9a9939b23b159d29")]
	public class ContextActionCastSpellSimple : ContextAction
	{
		// Token: 0x17001B74 RID: 7028
		// (get) Token: 0x0600AAA8 RID: 43688 RVA: 0x000726A4 File Offset: 0x000708A4
		public BlueprintAbility Spell
		{
			get
			{
				BlueprintAbilityReference spell = this.m_Spell;
				if (spell == null)
				{
					return null;
				}
				return spell.Get();
			}
		}

		// Token: 0x0600AAA9 RID: 43689 RVA: 0x002F3AD4 File Offset: 0x002F1CD4
		public override string GetCaption()
		{
			return string.Format("Cast spell {0}", this.Spell) + (this.OverrideDC ? string.Format(" DC: {0}", this.DC) : "") + (this.OverrideSpellLevel ? string.Format(" SL: {0}", this.SpellLevel) : "");
		}

		// Token: 0x0600AAAA RID: 43690 RVA: 0x002F3B34 File Offset: 0x002F1D34
		public override void RunAction()
		{
			if (base.Context.MaybeCaster == null)
			{
				Main.Log("Caster is missing");
				return;
			}
			AbilityData spell;
			if (!this.CastByTarget)
			{
				spell = new AbilityData(this.Spell, base.Context.MaybeCaster.Descriptor);
			}
			else
			{
				spell = new AbilityData(this.Spell, base.Target.Unit.Descriptor);
			}
			if (this.OverrideDC)
			{
				spell.OverrideDC = new int?(this.DC.Calculate(base.Context));
			}
			if (this.OverrideSpellLevel)
			{
				spell.OverrideSpellLevel = new int?(this.SpellLevel.Calculate(base.Context));
			}
		/*	if (!spell.CanTarget(base.Target))
			{
				Main.Log(string.Format("{0}: {1} is not valid target for {2}", this.name, base.Target.Point, this.Spell));
				return;
			}*/
			AbilityExecutionContext abilityContext = base.AbilityContext;
			bool flag = abilityContext != null && abilityContext.IsDuplicateSpellApplied;
			Rulebook.Trigger<RuleCastSpell>(new RuleCastSpell(spell, base.Target.Point)
			{
				IsDuplicateSpellApplied = flag
			});
		}

		// Token: 0x04007146 RID: 28998
		[SerializeField]
		[FormerlySerializedAs("Spell")]
		public BlueprintAbilityReference m_Spell;

		// Token: 0x04007147 RID: 28999
		public bool OverrideDC;

		// Token: 0x04007148 RID: 29000
		[ShowIf("OverrideDC")]
		public ContextValue DC;

		// Token: 0x04007149 RID: 29001
		public bool OverrideSpellLevel;

		// Token: 0x0400714A RID: 29002
		[ShowIf("OverrideSpellLevel")]
		public ContextValue SpellLevel;

		// Token: 0x0400714B RID: 29003
		public bool CastByTarget;
	}
}
