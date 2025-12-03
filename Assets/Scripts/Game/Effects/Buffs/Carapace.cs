using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Carapace: Effect
    {
        public Carapace(sbyte duration, PieceLogic piece) : base(duration, 1, piece, "effect_carapace")
        {}

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null || action.Target != Piece.Pos || !action.Succeed || (action.Flag & ActionFlag.Unblockable) != 0) return;
            
            action.Succeed = false;
            ActionManager.EnqueueAction(new CarapaceKill(Piece.Pos, action.Maker));
            ActionManager.EnqueueAction(new RemoveEffect(this));
            
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 80;
        }
    }
}