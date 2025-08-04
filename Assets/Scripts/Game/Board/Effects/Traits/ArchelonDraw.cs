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
            if (Distance(action.Target, Piece.Pos) > 2 || 
                ColorOfPiece(action.Target) != Piece.Color ||
                action.Target == Piece.Pos) 
                return;
            
            action.Target = Piece.Pos;
        }
    }
}