using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Buffs
{
    public class Carapace: Effect
    {
        public Carapace(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, EffectType.Carapace)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.To != Piece.pos || action.Success != ActionResult.Succeed) return;
            
            action.Success = ActionResult.Failed;
            ActionManager.EnqueueAction(new CarapaceKill(Piece.pos, action.From));
            ActionManager.EnqueueAction(new RemoveEffect(this));
            
        }
    }
}