using Game.Managers;


namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CorruptedWisperCharge : Effect, IEndTurnEffect
    {
        private readonly bool color;

        public CorruptedWisperCharge(int strength, bool color) : base(-1, strength, null, "effect_corrupted_wisper_charge")
        {
            this.color = color;
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (MatchManager.Ins.GameState.CurrentTurn % 10 == 0)
            {
                Strength++;
            }
        }
    }
}