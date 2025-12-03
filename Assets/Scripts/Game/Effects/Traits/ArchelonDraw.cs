using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArchelonDraw: Effect
    {
        public ArchelonDraw(PieceLogic piece) : base(-1, 1, piece, "effect_archelon_draw")
        {}

        public override void OnCallPieceAction(Action.Action action)
        {
            if (Distance(action.Target, Piece.Pos) > 2 || 
                ColorOfPiece(action.Target) != Piece.Color ||
                action.Target == Piece.Pos) 
                return;
            
            action.Target = Piece.Pos;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 100;
        }
    }
}