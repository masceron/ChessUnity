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
            if (Distance(action.To, Piece.pos) > 2 || 
                gameState.MainBoard[action.To].color != Piece.color ||
                action.To == Piece.pos) 
                return;
            
            action.To = Piece.pos;
        }
    }
}