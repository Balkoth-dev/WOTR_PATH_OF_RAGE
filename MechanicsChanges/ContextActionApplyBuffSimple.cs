using System;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace WOTR_PATH_OF_RAGE.MechanicsChanges
{
	// Token: 0x02001928 RID: 6440
	[TypeId("ccc303cd26554006bb4aef2c40b42a1c")]
	public class ContextActionApplyBuffSimple : ContextAction
	{
		// Token: 0x17001B72 RID: 7026
		// (get) Token: 0x0600AA8E RID: 43662 RVA: 0x000725E4 File Offset: 0x000707E4
		public BlueprintBuff Buff
		{
			get
			{
				BlueprintBuffReference buff = this.m_Buff;
				if (buff == null)
				{
					return null;
				}
				return buff.Get();
			}
		}

		// Token: 0x0600AA8F RID: 43663 RVA: 0x002F3294 File Offset: 0x002F1494
		public override string GetCaption()
		{
			string str = string.Concat(new string[]
			{
				"Apply",
				this.AsChild ? " child" : "",
				" Buff",
				this.ToCaster ? " to caster" : "",
				": ",
				this.Buff.NameSafe() ?? "???"
			});
			if (this.Permanent)
			{
				return str + " (permanent)";
			}
			string str2 = this.SameDuration ? "same duration" : (this.UseDurationSeconds ? string.Format("{0} seconds", this.DurationSeconds) : this.DurationValue.ToString());
			return str + " (for " + str2 + ")";
		}

		// Token: 0x0600AA90 RID: 43664 RVA: 0x002F336C File Offset: 0x002F156C
		public override void RunAction()
		{
			MechanicsContext.Data data = ContextData<MechanicsContext.Data>.Current;
			MechanicsContext context = (data != null) ? data.Context : null;
			if (context == null)
			{
				PFLog.Default.Error(this, "Unable to apply buff: no context found", Array.Empty<object>());
				return;
			}
			UnitEntityData buffTarget = this.GetBuffTarget(context);
			if (buffTarget == null)
			{
				PFLog.Default.Error(this, "Can't apply buff: target is null", Array.Empty<object>());
				return;
			}
			TimeSpan? duration = this.CalculateDuration(context);
			Buff buff = buffTarget.Descriptor.AddBuff(this.Buff, context, duration);
			if (buff == null)
			{
				return;
			}
			AreaEffectContextData areaEffectContextData = ContextData<AreaEffectContextData>.Current;
			AreaEffectEntityData entity = (areaEffectContextData != null) ? areaEffectContextData.Entity : null;
			if (entity != null)
			{
				buff.SourceAreaEffectId = entity.UniqueId;
			}
			buff.IsFromSpell = (this.IsFromSpell || this.Buff.IsFromSpell);
			buff.IsNotDispelable = this.IsNotDispelable;
			if (this.AsChild)
			{
				Buff.Data data2 = ContextData<Kingmaker.UnitLogic.Buffs.Buff.Data>.Current;
				Buff buff2 = (data2 != null) ? data2.Buff : null;
				if (buff2 != null)
				{
					if (buff2.Owner == buff.Owner)
					{
						buff2.StoreFact(buff);
						return;
					}
					PFLog.Default.Error(context.AssociatedBlueprint, "Parent and child buff must have one owner (" + context.AssociatedBlueprint.name + ")", Array.Empty<object>());
				}
			}
		}

		// Token: 0x0600AA91 RID: 43665 RVA: 0x000725F7 File Offset: 0x000707F7
		public UnitEntityData GetBuffTarget(MechanicsContext context)
		{
			if (!this.ToCaster)
			{
				return base.Target.Unit;
			}
			return context.MaybeCaster;
		}

		// Token: 0x0600AA92 RID: 43666 RVA: 0x002F34A0 File Offset: 0x002F16A0
		private TimeSpan? CalculateDuration(MechanicsContext context)
		{
			if (this.Permanent)
			{
				return null;
			}
			if (this.SameDuration)
			{
				Buff.Data data = ContextData<Kingmaker.UnitLogic.Buffs.Buff.Data>.Current;
				if (data != null)
				{
					Buff buff = data.Buff;
					if (buff != null)
					{
						return new TimeSpan?(buff.TimeLeft);
					}
				}
				return null;
			}
			TimeSpan value = this.UseDurationSeconds ? this.DurationSeconds.Seconds() : this.DurationValue.Calculate(context).Seconds;
			return new TimeSpan?(value);
		}

		// Token: 0x0400712E RID: 28974
		[SerializeField]
		[FormerlySerializedAs("Buff")]
		public BlueprintBuffReference m_Buff;

		// Token: 0x0400712F RID: 28975
		public bool Permanent;

		// Token: 0x04007130 RID: 28976
		public bool UseDurationSeconds;

		// Token: 0x04007131 RID: 28977
		[HideIf("UseDurationSeconds")]
		public ContextDurationValue DurationValue;

		// Token: 0x04007132 RID: 28978
		[ShowIf("UseDurationSeconds")]
		public float DurationSeconds;

		// Token: 0x04007133 RID: 28979
		public bool IsFromSpell;

		// Token: 0x04007134 RID: 28980
		public bool IsNotDispelable;

		// Token: 0x04007135 RID: 28981
		public bool ToCaster;

		// Token: 0x04007136 RID: 28982
		public bool AsChild = true;

		// Token: 0x04007137 RID: 28983
		public bool SameDuration;
	}
}
