using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Augmentation
{
    public class NerveCoolantPassive : Effect, IOnApplyTrigger
    {
        public NerveCoolantPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_nerve_coolant_passive")
        {
        }

        public void OnApply()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new QuickReflex(Piece)));
        }
    }
}