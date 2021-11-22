using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace WOTR_PATH_OF_RAGE.New_Rules
{
    [TypeId("a1aef9afc9b44bf19688535a56c59e40")]
    class ContextActionIncreaseRageRounds : ContextAction
    {
        public BlueprintAbilityResourceReference m_resource;
        public int m_resourceAmount;
        public override string GetCaption()
        {
            return string.Format("Increases your Demon Rage rounds");
        }

        public override void RunAction()
        {
            var caster = base.Context.MaybeCaster;
            if (caster == null) { return; }
            if (m_RandomChance)
            {
                var randomInt = UnityEngine.Random.Range(1, 101);
                if (randomInt <= m_RandomChancePercent)
                {
                    caster.Resources.Restore(m_resource, m_resourceAmount);
                }
            }
            else
            {
                caster.Resources.Restore(m_resource, m_resourceAmount);
            }
        }

        public bool m_RandomChance;
        public int m_RandomChancePercent;

    }
}