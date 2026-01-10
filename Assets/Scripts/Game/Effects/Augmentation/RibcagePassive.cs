using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class RibcagePassive : Effect, IAfterPieceActionEffect
    {
        public RibcagePassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_ribcage_passive")
        { }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            
            ActionManager.EnqueueAction(new ApplyEffect(new LongReach(Piece, 1, -1)));
        }
    }
}