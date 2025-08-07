using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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