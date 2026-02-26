using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Augmentation
{
    public class PredatorEyesPassive : Effect, IOnApplyTrigger
    {
        public PredatorEyesPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_predator_eyes_passive")
        {
        }

        public void OnApply()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Ambush(Piece)));
        }
    }
}