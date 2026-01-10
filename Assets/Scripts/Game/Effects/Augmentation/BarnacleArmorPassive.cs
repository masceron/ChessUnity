using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class BarnacleArmorPassive : Effect, IOnApply
    {
        public BarnacleArmorPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_barnacle_armor_passive")
        { }

        public void OnApply()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Extremophile(Piece)));
        }
    }
}