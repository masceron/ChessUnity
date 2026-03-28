using Game.Action.Skills;
using Game.Triggers;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Charge : Effect, IEndTurnTrigger
    {
        private readonly bool color;

        public Charge(int strength, bool color) : base(-1, strength, null, "effect_charge")
        {
            this.color = color;
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Other;

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (lastMainAction is ISkills && lastMainAction.GetMaker() != null &&
                lastMainAction.GetMaker().Color != color) Strength++;
            if (Strength > 3) Strength = 3;
        }
    }
}