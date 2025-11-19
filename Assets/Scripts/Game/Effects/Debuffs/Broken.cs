using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Broken: Effect
    {
        public Broken(sbyte duration, PieceLogic piece) : base(duration, 1, piece, "effect_broken")
        {
            
        }

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null || action.Target != Piece.Pos || action.Result != ActionResult.Succeed) return;
            
            // if (IsPassive)
            //     action.Result = ActionResult.Failed;
            ActionManager.EnqueueAction(new CarapaceKill(Piece.Pos, action.Maker));
            ActionManager.EnqueueAction(new RemoveEffect(this));
        }
    }
}