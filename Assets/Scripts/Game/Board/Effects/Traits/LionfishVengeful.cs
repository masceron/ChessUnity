using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.Piece.PieceLogic;
using Game.Common;

namespace Game.Board.Effects.Traits
{
    public class LionfishVengeful: Effect
    {
        public LionfishVengeful(PieceLogic piece) : base(-1, 1, piece, EffectName.LionfishVengeful)
        {}
        
        public override void OnCall(Action.Action action)
        {
            if (action == null) return;
            
            if (action.To == Piece.Pos && action.Result != ActionResult.Failed)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(3, BoardUtils.PieceOn(action.Caller))));
            }
        }
    }
}