using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
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