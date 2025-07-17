using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Buffs
{
    public class Shield: Effect
    {
        public Shield(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectType.Shield)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.To != Piece.pos || action.Success != ActionResult.Succeed) return;
            action.Success = ActionResult.Failed;
            ActionManager.EnqueueAction(new RemoveEffect(this));
        }
    }
}