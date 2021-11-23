using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOTR_PATH_OF_RAGE.MechanicsChanges
{
		// Token: 0x02001E8A RID: 7818
		[AllowedOn(typeof(BlueprintUnitFact), false)]
		[TypeId("cfc0fd39107c47cca2d3c2b6c1ab6540")]
		public class BuffExtraAttackDemon : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttacksCount>, IRulebookHandler<RuleCalculateAttacksCount>, ISubscriber, IInitiatorRulebookSubscriber
		{
			// Token: 0x0600C461 RID: 50273 RVA: 0x00081A38 File Offset: 0x0007FC38
			public void OnEventAboutToTrigger(RuleCalculateAttacksCount evt)
			{
				int bonus = ((int)Math.Floor(base.Owner.Progression.MythicLevel / 3.0f));
				evt.AddExtraAttacks(bonus, this.Haste, null);
			}

			// Token: 0x0600C462 RID: 50274 RVA: 0x000031E7 File Offset: 0x000013E7
			public void OnEventDidTrigger(RuleCalculateAttacksCount evt)
			{
			}

			// Token: 0x04008078 RID: 32888
			public bool Haste;
		}
}
