using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class DuelistReflexPassive : Effect, IAfterPieceActionEffect, IBeforePieceActionEffect
    {
        

        public DuelistReflexPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece, "effect_duelist_reflex_passive")
        { }


        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.Target != Piece.Pos) return;
            if (action.Result != ResultFlag.Success) return;
            if ((action.Flag & ActionFlag.Unblockable) != 0) return;

            var probability = UnityEngine.Random.Range(0, 101);
            if (probability >= 50)
            {
                action.Result = ResultFlag.Parry;
            }
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.Target != Piece.Pos) return;
            if (action.Result != ResultFlag.Parry) return;

            ActionManager.EnqueueAction(new KillPiece(action.Maker));
        }
    }
}