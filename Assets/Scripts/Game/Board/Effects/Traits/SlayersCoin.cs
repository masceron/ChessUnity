using Game.Board.Action;
using Game.Board.Piece.PieceLogic;
using Game.Board.Piece.PieceLogic.Commanders;
using static Game.Board.General.MatchManager;

namespace Game.Board.Effects.Traits
{
    public class SlayersCoin: Effect
    {
        public SlayersCoin(PieceLogic piece) : base(-1, 1, piece, EffectType.SlayersCoin)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.Result == ActionResult.Failed) return;
            
            var caller = gameState.MainBoard[action.Caller];
            var captured = gameState.MainBoard[action.To];
            
            if (caller.color == Piece.color && caller.pieceRank < captured.pieceRank) ((Chrysos)Piece).Coin += 1;
        }

        public override string Description()
        {
            return string.Format(assetManager.EffectData[EffectName].description, ((Chrysos)Piece).Coin);
        }
    }
}