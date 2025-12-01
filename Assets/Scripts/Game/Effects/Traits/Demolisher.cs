using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Demolisher: Effect
    {
        public Demolisher(PieceLogic piece) : base(-1, 1, piece, "effect_demolisher")
        {}

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action.GetType() == typeof(DestroyConstruct) && action.Maker == Piece.Pos && action.Succeed)
            {
                ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 0;
        }
    }
}