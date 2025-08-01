using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Traits
{
    public class Ambush: Effect, IEndTurnEffect
    {
        private byte lastUsed;
        private bool active;
        private const sbyte RangeOffset = 2;

        public Ambush(sbyte duration, PieceLogic piece) : base(duration, -1, piece, EffectName.Ambush)
        {
            EndTurnEffectType = EndTurnEffectType.AtAllyTurn;
        }

        public void OnCallEnd(Action.Action action)
        {
            if (action == null) return;
            if (action.Caller != Piece.Pos)
            {
                lastUsed++;
                if (lastUsed < 6 || active) return;
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
    }
}