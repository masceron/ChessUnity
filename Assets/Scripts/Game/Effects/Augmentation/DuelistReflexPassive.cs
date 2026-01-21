using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class DuelistReflexPassive : Effect, IAfterPieceActionEffect
    {
        

        public DuelistReflexPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, "effect_duelist_reflex_passive")
        { }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action == null || action is not ICaptures) return;
            
            if (action.Target != Piece.Pos) return;
            
            var probability = UnityEngine.Random.Range(0, 101);
            if (probability >= 50)
            {
                ActionManager.EnqueueAction(new KillPiece(action.Maker));
            }
        }
    }
}