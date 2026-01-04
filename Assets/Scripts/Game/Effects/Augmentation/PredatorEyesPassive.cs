using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class PredatorEyesPassive : Effect, IOnApply
    {
        public PredatorEyesPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_predator_eyes_passive")
        { }

        public void OnApply()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Ambush(Piece)));
        }
    }
}