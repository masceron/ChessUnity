using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrenziedVeteran : Effect, IEndTurnTrigger, IAttackRangeModifier
    {
        public const byte TurnsToActive = 10;
        private const byte RangeOffset = 1;
        public byte numTurns;

        public FrenziedVeteran(PieceLogic piece) : base(-1, -1, piece, "effect_frenzied_veteran")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public int ModifyAttackRange(int baseRange)
        {
            return baseRange + numTurns / 10; // cứ 10 turn tầm đánh tăng 1
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            numTurns++;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Buff;

        public EndTurnEffectType EndTurnEffectType { get; set; }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 60;
        }
    }
}