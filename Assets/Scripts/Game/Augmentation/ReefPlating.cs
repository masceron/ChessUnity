using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    public class ReefPlating : Augmentation
    {
        public ReefPlating() : base(AugmentationName.ReefPlating, AugmentationRarity.Basic, AugmentationSlot.Chassis,
            null, null)
        {
            PassiveEffects = new List<Effect>();
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.None, false);
            PassiveEffects.Add(new ReefPlatingPassive(Target));
        }
    }
}