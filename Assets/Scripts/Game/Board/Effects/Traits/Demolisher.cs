using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Traits
{
    public class Demolisher: Effect
    {
        public Demolisher(PieceLogic piece) : base(-1, 1, piece, EffectName.Demolisher)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.GetType() == typeof(DestroyConstruct) && action.Maker == Piece.Pos)
            {
                ActionManager.EnqueueAction(new DestroyPiece(Piece.Pos));
            }
        }
    }
}