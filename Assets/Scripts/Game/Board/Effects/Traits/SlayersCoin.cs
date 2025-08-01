using Game.Board.Action;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;
using Game.Board.Piece.PieceLogic.Commanders;

namespace Game.Board.Effects.Traits
{
    public class SlayersCoin: Effect
    {
        public SlayersCoin(PieceLogic piece) : base(-1, 1, piece, EffectName.SlayersCoin)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.Result == ActionResult.Failed) return;
            
            var caller = MatchManager.Ins.GameState.PieceBoard[action.Caller];
            var captured = MatchManager.Ins.GameState.PieceBoard[action.To];
            
            if (caller.Color == Piece.Color && caller.PieceRank < captured.PieceRank) ((Chrysos)Piece).Coin += 1;
        }

        public override string Description()
        {
            return string.Format(AssetManager.Ins.EffectData[EffectName].description, ((Chrysos)Piece).Coin);
        }
    }
}