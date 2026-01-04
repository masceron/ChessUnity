using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    public class ShadowFin : Augmentation
    {
        public ShadowFin() : base(AugmentationName.ShadowFin, AugmentationRarity.Rare, AugmentationSlot.Fin, null, null)
        {
            PassiveEffects = new List<Effect>();
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.None, false);
            // TODO: Change AugmentationSetType to Spectre.
            PassiveEffects.Add(new ShadowFinPassive(Target));
        }
    }
}