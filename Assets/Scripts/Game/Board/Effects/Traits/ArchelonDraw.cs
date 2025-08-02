using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Effects.Traits
{
    public class ArchelonDraw: Effect
    {
        public ArchelonDraw(PieceLogic piece) : base(-1, 1, piece, EffectName.ArchelonDraw)
        {}

        public override void OnCall(Action.Action action)
        {
            if (Distance(action.To, Piece.Pos) > 2 || 
                ColorOfPiece(action.To) != Piece.Color ||
                action.To == Piece.Pos) 
                return;
            
            action.To = Piece.Pos;
        }
    }
}