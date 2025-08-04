using Game.Board.Action;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Buffs
{
    public class Piercing: Effect
    {
        public Piercing(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectName.Piercing)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action != null && action.Maker == Piece.Pos) action.Result = ActionResult.Unblockable;
        }
    }
}