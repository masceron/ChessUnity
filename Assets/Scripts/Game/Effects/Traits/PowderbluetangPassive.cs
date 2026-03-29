using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using Game.Triggers;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PowderbluetangPassive : Effect, IAfterPieceActionTrigger
    {
        private int _count;

        public PowderbluetangPassive(PieceLogic piece) : base(-1, 1, piece, "effect_powderbluetang_passive")
        {
            _count = 2;
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action.GetMaker() == Piece &&
                action is NormalMove &&
                action.Result == ResultFlag.Success &&
                _count > 0 &&
                BoardUtils.GetFormation(action.GetTargetPos()) is HydroidThicket)
            {
                _count--;
                ActionManager.EnqueueAction(new ApplyEffect(new HardenedShield(Piece)));
            }
        }
    }
}