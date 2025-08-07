using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Carapace: Effect
    {
        public Carapace(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectName.Carapace)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action == null || action.Target != Piece.Pos || action.Result != ActionResult.Succeed) return;
            
            action.Result = ActionResult.Failed;
            ActionManager.EnqueueAction(new CarapaceKill(Piece.Pos, action.Maker));
            ActionManager.EnqueueAction(new RemoveEffect(this));
            
        }
    }
}