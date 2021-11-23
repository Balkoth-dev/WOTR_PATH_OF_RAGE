using System;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.UI.Log;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace WOTR_PATH_OF_RAGE.MechanicsChanges
{
	// Token: 0x02001A47 RID: 6727
	[AllowedOn(typeof(BlueprintAbility), false)]
	[AllowMultipleComponents]
	[TypeId("9cdd5e74ecf141e7ac9e50ac777a30a5")]
	public class AbilityCasterSecondaryWeaponCheck : BlueprintComponent, IAbilityCasterRestriction
	{
		// Token: 0x0600AFD2 RID: 45010 RVA: 0x00075BF0 File Offset: 0x00073DF0
		public bool IsCasterRestrictionPassed(UnitEntityData caster)
		{
			return caster.Body.SecondaryHand.HasWeapon && this.Category.Contains(caster.Body.SecondaryHand.Weapon.Blueprint.Type.Category);
		}

		// Token: 0x0600AFD3 RID: 45011 RVA: 0x00302DE0 File Offset: 0x00300FE0
		public string GetAbilityCasterRestrictionUIText()
		{
			string categories = string.Join(", ", this.Category.Select(new Func<WeaponCategory, string>(LocalizedTexts.Instance.WeaponCategories.GetText)));
			return LocalizedTexts.Instance.Reasons.SpecificWeaponRequired.ToString(delegate ()
			{
				GameLogContext.Text = categories;
			});
		}

		// Token: 0x0400742D RID: 29741
		public WeaponCategory[] Category;
	}
}
