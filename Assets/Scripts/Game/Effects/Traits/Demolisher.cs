using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Demolisher: Effect, IAfterPieceActionEffect
    {
        public Demolisher(PieceLogic piece) : base(-1, 1, piece, "effect_demolisher")
        {}

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is DestroyConstruct && action.Maker == Piece.Pos && action.Result == ResultFlag.Success)
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