using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOTR_PATH_OF_RAGE.Utilities;

namespace WOTR_PATH_OF_RAGE.MechanicsChanges
{
    class BlueprintAbilityPatch
    {
        [HarmonyPatch(typeof(BlueprintAbility), "IsInSpellList")]
        static class BlueprintAbilityPatch_IsInSpellList_Patch
        {
            static void Postfix(BlueprintSpellbook spellbook, BlueprintSpellList spellList, ref bool __result, BlueprintAbility __instance)
            {
				try
				{
					SpellLevelList[] spellsByLevel = spellList.SpellsByLevel;

					for (int i = 0; i < spellList.MaxLevel; i++)
					{
						foreach(var spell in spellList.GetSpells(i))
						{
							if (spell == __instance)
							{
								__result = true;
								return;
							}
							AbilityVariants spellComponent = spell.m_AbilityVariants;
							ReferenceArrayProxy<BlueprintAbility, BlueprintAbilityReference>? referenceArraySpell = (spellComponent != null) ? new ReferenceArrayProxy<BlueprintAbility, BlueprintAbilityReference>?(spellComponent.Variants) : null;
							AbilityVariants instanceComponent = __instance.m_AbilityVariants;
							ReferenceArrayProxy<BlueprintAbility, BlueprintAbilityReference>? referenceArrayInstance = (instanceComponent != null) ? new ReferenceArrayProxy<BlueprintAbility, BlueprintAbilityReference>?(instanceComponent.Variants) : null;
							if (referenceArraySpell != null)
							{
								foreach (var spellVariant in referenceArraySpell)
								{
									foreach (var instanceVariant in referenceArrayInstance)
									{
										if (spellVariant == instanceVariant)
										{
											__result = true;
											return;
										}
									}
								}
							}
						}
					}

					__result = false;
				}
				finally
				{
				}
			}
        }
    }
}
