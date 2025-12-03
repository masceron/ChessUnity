using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrenziedVeteran: Effect, IEndTurnEffect
    {
        public const byte TurnsToActive = 10;
        public byte numTurns;
        private bool active;
        private const byte RangeOffset = 1;

        public FrenziedVeteran(PieceLogic piece) : base(-1, -1, piece, "effect_frenzied_veteran")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            numTurns++;
            if (numTurns % TurnsToActive == 0)
            {
                Piece.AttackRange++;
            }
        }

        public override void OnRemove()
        {
            if (active) Piece.AttackRange -= RangeOffset;
        }

        public EndTurnEffectType EndTurnEffectType { get; set; }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 60;
        }
    }
}