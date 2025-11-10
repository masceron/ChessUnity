using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class LionfishVengeful: Effect
    {
        public LionfishVengeful(PieceLogic piece) : base(-1, 1, piece, EffectName.LionfishVengeful)
        {}
        
        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null) return;
            
            if (action.Target == Piece.Pos && action.Result == ActionResult.Succeed)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(3, BoardUtils.PieceOn(action.Maker))));
            }
        }
    }
}