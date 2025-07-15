using Game.Board.Action;

namespace Game.Board.Effects.Buffs
{
    public class Piercing: Effect
    {
        public Piercing(sbyte duration, PieceLogic.PieceLogic piece) : base(duration, 1, piece, EffectType.Piercing)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.Caller == Piece.pos) action.Success = ActionResult.Unblockable;
        }
    }
}