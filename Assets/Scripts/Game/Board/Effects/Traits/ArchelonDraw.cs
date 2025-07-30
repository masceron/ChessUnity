using Game.Board.Piece.PieceLogic;
using static Game.Board.General.MatchManager;
using static Game.Common.BoardUtils;

namespace Game.Board.Effects.Traits
{
    public class ArchelonDraw: Effect
    {
        public ArchelonDraw(PieceLogic piece) : base(-1, 1, piece, EffectType.ArchelonDraw)
        {}

        public override string Description()
        {
            return assetManager.EffectData[EffectName].description;
        }

        public override void OnCall(Action.Action action)
        {
            if (Distance(action.To, Piece.Pos) > 2 || 
                gameState.PieceBoard[action.To].Color != Piece.Color ||
                action.To == Piece.Pos) 
                return;
            
            action.To = Piece.Pos;
        }
    }
}