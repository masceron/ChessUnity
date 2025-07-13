using Game.Board.General;

namespace Game.Board.Effects
{
    public class Ambush: Effect
    {
        private byte lastUsed;
        private bool active;
        private const sbyte RangeOffset = 1;

        public Ambush(sbyte duration, PieceLogic.PieceLogic piece) : base(duration, -1, piece, ObserverType.EndTurn, 0)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action == null) return;
            if (action.Caller != Piece.pos)
            {
                lastUsed++;
                if (lastUsed >= 6 && !active)
                {
                    active = true;
                    Piece.attackRange += RangeOffset;
                }
            }
            else if (active)
            {
                active = false;
                lastUsed = 0;
                Piece.attackRange -= RangeOffset;
            }
        }

        public override void OnRemove()
        {
            if (active) Piece.attackRange -= RangeOffset;
        }
    }
}