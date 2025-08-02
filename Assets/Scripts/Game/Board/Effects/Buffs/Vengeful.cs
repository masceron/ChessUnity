using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.Piece.PieceLogic;
using Game.Common;

namespace Game.Board.Effects.Buffs
{
    public class Vengeful: Effect
    {
        public Vengeful(PieceLogic piece) : base(-1, 1, piece, EffectName.Vengeful)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action == null) return;
            
            if (action.To == Piece.Pos && action.Result != ActionResult.Failed)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(3, BoardUtils.PieceOn(action.To))));
            }
        }
    }
}