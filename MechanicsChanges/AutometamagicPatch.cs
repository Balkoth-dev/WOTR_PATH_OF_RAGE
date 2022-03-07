using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOTR_PATH_OF_RAGE.Utilities;

namespace WOTR_PATH_OF_RAGE.MechanicsChanges
{
    class AutometamagicPatch
    {
        [HarmonyPatch(typeof(AutoMetamagic), "ShouldApplyTo")]
        static class AutometamagicPatch_ShouldApplyTo_Patch
        {
            static void Postfix(AutoMetamagic c, [NotNull] BlueprintAbility ability, [CanBeNull] AbilityData data, ref bool __result)
            {
                if (Main.settings.PatchAutometamagic == false)
                {
                    return;
                }
                else
                {
                    BlueprintAbility blueprintAbility1;
                    if ((object)data == null)
                    {
                        blueprintAbility1 = (BlueprintAbility)null;
                    }
                    else
                    {
                        AbilityData convertedFrom = data.ConvertedFrom;
                        if ((object)convertedFrom == null)
                        {
                            blueprintAbility1 = (BlueprintAbility)null;
                        }
                        else
                        {
                            blueprintAbility1 = convertedFrom.Blueprint.Or<BlueprintAbility>((BlueprintAbility)null);
                            if (blueprintAbility1 != null)
                                goto a9ac2d7f62db4920896cc8732b1a05a4;
                        }
                    }
                    BlueprintAbility blueprintAbility2 = ability.Or<BlueprintAbility>((BlueprintAbility)null);
                    BlueprintAbility blueprintAbility3;
                    if (blueprintAbility2 == null)
                    {
                        blueprintAbility3 = (BlueprintAbility)null;
                    }
                    else
                    {
                        blueprintAbility3 = blueprintAbility2.Parent.Or<BlueprintAbility>((BlueprintAbility)null);
                        if (blueprintAbility3 != null)
                        {
                            blueprintAbility1 = blueprintAbility3;
                            goto a9ac2d7f62db4920896cc8732b1a05a4;
                        }
                    }
                    blueprintAbility1 = ability;
                a9ac2d7f62db4920896cc8732b1a05a4:
                    BlueprintAbility ability1 = blueprintAbility1;
                    if (ability1 != ability && AutoMetamagic.ShouldApplyTo(c, ability1, (AbilityData)null))
                    {
                        __result = true;
                        return;
                    }
                    if (c.IsSuitableAbility(ability, data) && (c.Abilities.Empty<BlueprintAbilityReference>() || c.Abilities.HasItem<BlueprintAbilityReference>((Func<BlueprintAbilityReference, bool>)(r => r.Is(ability)))) && (c.Descriptor == SpellDescriptor.None || ability.SpellDescriptor.HasAnyFlag((SpellDescriptor)c.Descriptor)) && (c.School == SpellSchool.None || ability.School == c.School))
                    {
                        if (c.MaxSpellLevel >= 1)
                        {
                            int? spellLevel = data?.SpellLevel;
                            int maxSpellLevel = c.MaxSpellLevel;
                            if (!(spellLevel.GetValueOrDefault() <= maxSpellLevel & spellLevel.HasValue))
                                goto a21e1761dce7f4fdd8dad837b396cd16;
                        }
                        //__result = !c.CheckSpellbook || ability.IsInSpellList(c.Spellbook.SpellList);
                        if (c.CheckSpellbook)
                        {
                            if ((object)data == null)
                            {
                                __result = false;
                            }
                            else
                            {
                                AbilityData convertedFrom = data.ConvertedFrom;
                                if ((object)convertedFrom == null)
                                {
                                    if (data.SpellbookBlueprint == c.Spellbook)
                                    {
                                        __result = true;
                                    }
                                }
                                else
                                {
                                    if (convertedFrom.SpellbookBlueprint == c.Spellbook)
                                    {
                                        __result = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            __result = true;
                        }
                        return;
                    }
                }
            a21e1761dce7f4fdd8dad837b396cd16:
                __result = false;
                return;
            }
        }
    }
}
