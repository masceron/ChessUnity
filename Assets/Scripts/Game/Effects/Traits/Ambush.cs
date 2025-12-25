using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Ambush: Effect, IEndTurnEffect
    {
        private byte lastUsed;
        private bool active;
        private const byte RangeOffset = 2;
        private const byte TurnsToActive = 3;

        public Ambush(PieceLogic piece) : base(-1, -1, piece, "effect_ambush")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public void OnCallEnd(Action.Action action)
        {
            if (action.Maker != Piece.Pos)
            {
                lastUsed++;
                if (lastUsed < TurnsToActive || active) return;
                active = true;
                Piece.AttackRange += RangeOffset;
            }
            else if (active)
            {
                active = false;
                lastUsed = 0;
                Piece.AttackRange -= RangeOffset;
            }
        }

        public override void OnRemove()
        {
            if (active) Piece.AttackRange -= RangeOffset;
        }

        public EndTurnEffectType EndTurnEffectType { get; set; }
        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 30;
        }
    }
}