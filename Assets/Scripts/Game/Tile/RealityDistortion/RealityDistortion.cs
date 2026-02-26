using Game.Triggers;

namespace Game.Tile.RealityDistortion
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RealityDistortion : Formation, IStartTurnTrigger
    {
        public RealityDistortion(bool haveDuration, bool color) : base(color)
        {
            HaveDuration = haveDuration;
        }

        public void OnCallStart(Action.Action lastMainAction)
        {
            if (!RealityDistortionManager.Ins) return;
            RealityDistortionManager.Ins.OnTurnStart();
        }

        public new StartTurnTriggerPriority Priority => StartTurnTriggerPriority.FormationOther;

        public StartTurnEffectType StartTurnEffectType => StartTurnEffectType.StartOfAnyTurn;

        public override FormationType GetFormationType()
        {
            return FormationType.RealityDistortion;
        }

        public override int GetValueForAI()
        {
            return -10;
        }
    }
}