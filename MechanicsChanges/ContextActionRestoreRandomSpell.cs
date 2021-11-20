using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using UnityEngine;

namespace WOTR_PATH_OF_RAGE.MechanicsChanges
{
	// Token: 0x02001965 RID: 6501
	[TypeId("f480289661514196bc0a6af195e6f4f0")]
	public class ContextActionRestoreRandomSpell : ContextAction
	{

		// Token: 0x0600AB74 RID: 43892 RVA: 0x00072CBA File Offset: 0x00070EBA
		public override string GetCaption()
		{
			return "Restore all spells in spellbooks";
		}

		// Token: 0x0600AB75 RID: 43893 RVA: 0x002F7030 File Offset: 0x002F5230
		public override void RunAction()
		{
			if (base.Target.Unit == null)
			{
				Main.Log("Can't be applied to point");
				return;
			}
			if (base.Context.MaybeCaster == null)
			{
				Main.Log("Caster is missing");
				return;
			}
			var maxSpellLevel = base.Context.MaybeCaster.DemandSpellbook(m_Spellbook).GetMaxSpellLevel();

			var randomInt = UnityEngine.Random.Range(1,maxSpellLevel+1);
			base.Context.MaybeCaster.DemandSpellbook(m_Spellbook).RestoreSpontaneousSlots(randomInt, m_Amount);
			
		}

		// Token: 0x040071CF RID: 29135
		[SerializeField]
		public BlueprintSpellbookReference m_Spellbook = new BlueprintSpellbookReference();

		public int m_Amount = new();
	}
}
