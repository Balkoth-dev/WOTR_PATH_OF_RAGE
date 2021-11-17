using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace WOTR_PATH_OF_RAGE.New_Rules
{
    [TypeId("114f3881649e4cb2a9e2e0693f1ae5e0")]
    class ContextActionIncreaseRageRounds : ContextAction
    {
        public override string GetCaption()
        {
            return string.Format("Increases your Demon Rage rounds");
        }

        public override void RunAction()
        {
            var Caster = base.Context.MaybeCaster;
            if (Caster == null) { return; }
            Caster.Resources.Restore(m_destinationResource, m_destinationAmount);
        }

        public BlueprintAbilityResourceReference m_sourceResource;
        public BlueprintAbilityResourceReference m_destinationResource;
        public int m_sourceAmount;
        public int m_destinationAmount;
        public ActionList FailedActions;
        public ContextValue SaveDC;
    }
}