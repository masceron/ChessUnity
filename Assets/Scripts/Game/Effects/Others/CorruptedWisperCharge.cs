using Game.Common;
using Game.Triggers;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CorruptedWisperCharge : Effect, IEndTurnTrigger
    {
        public CorruptedWisperCharge(int strength, bool color) : base(-1, strength, null,
            "effect_corrupted_wisper_charge")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Other;

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (BoardUtils.GetCurrentTurn() % 10 == 0) Strength++;
        }
    }
}