using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ElectricEelVengeful: Effect
    {
        public ElectricEelVengeful(PieceLogic piece) : base(-1, 1, piece, EffectName.ElectricEelVengeful)
        {}

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null) return;
            
            if (action.Target == Piece.Pos && action.Result == ActionResult.Succeed)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(2, BoardUtils.PieceOn(action.Maker))));
            }
        }
    }
}