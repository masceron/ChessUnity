using Game.Action.Skills;
using Game.Common;


namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Charge : Effect, IEndTurnEffect
    {
        private readonly bool color;

        public Charge(int strength, bool color) : base(-1, strength, null, "effect_charge")
        {
            this.color = color;
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (lastMainAction is ISkills && BoardUtils.PieceOn(lastMainAction.Maker) != null &&
                BoardUtils.PieceOn(lastMainAction.Maker).Color != color) Strength++;
            if (Strength > 3) Strength = 3;
        }
    }
}