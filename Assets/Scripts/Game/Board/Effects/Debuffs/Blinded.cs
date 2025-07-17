using Game.Board.Action;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Debuffs
{
    public class Blinded: Effect
    {
        public Blinded(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, EffectType.Blinded)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.Caller != Piece.pos || action.Success != ActionResult.Succeed) return;
            
            if (MatchManager.Roll(Strength))
            {
                action.Success = ActionResult.Failed;
            }
        }
    }
}