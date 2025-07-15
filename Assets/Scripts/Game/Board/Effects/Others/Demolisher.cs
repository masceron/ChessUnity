using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;

namespace Game.Board.Effects.Others
{
    public class Demolisher: Effect
    {
        public Demolisher(PieceLogic.PieceLogic piece) : base(-1, 1, piece, EffectType.Demolisher)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.GetType() == typeof(DestroyConstruct) && action.Caller == Piece.pos)
            {
                ActionManager.EnqueueAction(new SelfDestruct(Piece.pos));
            }
        }
    }
}